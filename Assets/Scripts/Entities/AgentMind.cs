using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AgentMind : MonoBehaviour
{
	// RL Components
	private Dictionary<AgentStateActionPair, float> qTable = new Dictionary<AgentStateActionPair, float>();
	private List<BaseAction> availableActions = new List<BaseAction>();
	private float reward;
	public AgentState currentState;
	public BaseAction currentAction;

	// Learning parameters
	public float learningRate = 0.1f;
	public float discountFactor = 0.9f; // value close to 1 encourages long-term rewards.
	public float explorationRate = 0.2f; // For epsilon-greedy exploration
	private float qTableInitValue = 0f;
	private float decisionMakingCoroutineInterval = 5f;

	private void Start()
	{
		// Collect and Initialize available actions
		availableActions.AddRange(GetComponents<BaseAction>());
		foreach (var action in availableActions)
		{
			action.Initialize();
		}

		AgentController agentController = GetComponent<AgentController>();
		currentState = GetNewState(agentController);
		currentAction = SelectAction(currentState);

		StartCoroutine(DecisionMakingCoroutine());
	}

	private void Update()
	{
		// Execute the action
		if (currentAction.CanExecute(currentState))
		{
			currentAction.Execute();
		}
	}

	private IEnumerator DecisionMakingCoroutine()
	{
		AgentController agentController = GetComponent<AgentController>();
		while (true)
		{
			// Decision-making logic
			DecideAction(agentController);

			// Wait for a fixed interval
			yield return new WaitForSeconds(this.decisionMakingCoroutineInterval);
		}
	}

	void OnDrawGizmos()
	{
		// Define the position above or next to the agent
		Vector3 labelPosition = transform.position + Vector3.up * 0.5f;
		Handles.Label(labelPosition, $"Action: {currentAction}");
	}

	public void DecideAction(AgentController agentController)
	{
		// Observe the current state
		AgentState newState = GetNewState(agentController);
		// Receive reward from the last action
		reward = GetReward(currentState, newState); // FIXME
		// Update Q-value
		UpdateQValue(currentState, currentAction, reward, newState);
		// Decide next action
		currentState = newState;
		BaseAction nextAction = SelectAction(currentState);

		// Execute the action
		if (currentAction != null)
		{
			if (nextAction.ActionName != currentAction.ActionName)
			{
				currentAction.Finish();
				currentAction = nextAction;
			}

			currentAction.Execute();
		}
	}


	public AgentState GetNewState(AgentController agentController)
	{
		AgentState state = new AgentState();
		// Discretize hunger level
		if (agentController.currentSatiety > 70f)
			state.satietyLevel = SatietyLevel.High;
		else if (agentController.currentSatiety > 30f)
			state.satietyLevel = SatietyLevel.Medium;
		else
			state.satietyLevel = SatietyLevel.Low;

		// Discretize energy level
		if (agentController.currentEnergy > 70f)
			state.energyLevel = EnergyLevel.High;
		else if (agentController.currentEnergy > 30f)
			state.energyLevel = EnergyLevel.Medium;
		else
			state.energyLevel = EnergyLevel.Low;

		// Detect nearby objects using LookSkill
		state.detectedObjectsTypes = new List<DetectedObjectType>();
		LookSkill lookSkill = GetComponent<LookSkill>();
		foreach (var detected in lookSkill.detectedObjects)
		{
			if (detected.Object.CompareTag("Food"))
				state.detectedObjectsTypes.Add(DetectedObjectType.Food);
			else if (detected.Object.CompareTag("Predator"))
				state.detectedObjectsTypes.Add(DetectedObjectType.Predator);
			// Add other object types as needed
		}

		if (state.detectedObjectsTypes.Count == 0)
		{
			state.detectedObjectsTypes.Add(DetectedObjectType.None);
		}

		return state;
	}

	public BaseAction SelectAction(AgentState state)
	{
		List<BaseAction> executableActions = new List<BaseAction>();

		// Filter actions that can be executed in the current state
		foreach (var action in availableActions)
		{
			if (action.CanExecute(state))
			{
				executableActions.Add(action);
			}
		}

		// Epsilon-greedy strategy remains the same
		if (Random.value < explorationRate)
		{
			// Random action (exploration)
			return executableActions[Random.Range(0, executableActions.Count)];
		}
		else if (state == null)		
		{
			return executableActions[0];
		}
		else
		{
			// Best action based on Q-values (exploitation)
			BaseAction bestAction = executableActions[0];
			float maxQ = float.MinValue;
			foreach (var action in executableActions)
			{
				AgentStateActionPair key = new AgentStateActionPair { state = state, action = action };
				float q = qTable.ContainsKey(key) ? qTable[key] : qTableInitValue;

				if (q > maxQ)
				{
					maxQ = q;
					bestAction = action;
				}
			}

			return bestAction;
		}
	}

	private float GetReward(AgentState previousState, AgentState newState)
	{
		// FIXME: reward multipliers are arbitrary and given from o1
		float reward = 0f;

		if (previousState == null || newState == null)
			return 0.1f;

		// reward for decreasing hunger
		if (newState.satietyLevel > previousState.satietyLevel)
			reward += (previousState.satietyLevel - newState.satietyLevel) * 2f;
		else if (newState.satietyLevel < previousState.satietyLevel)
			reward -= (newState.satietyLevel - previousState.satietyLevel) * 2;

		// reward for increasing energy
		if (newState.energyLevel > previousState.energyLevel)
			reward += (previousState.energyLevel - newState.energyLevel) * 2f;
		else if (newState.energyLevel < previousState.energyLevel)
			reward -= (newState.energyLevel - previousState.energyLevel) * 2f;

		// Negative reward for being near a predator
		// FIXME: near!=seen
		if (newState.detectedObjectsTypes.Contains(DetectedObjectType.Predator))
			reward -= 5f;
		if (newState.detectedObjectsTypes.Contains(DetectedObjectType.Food))
			reward += 2f;

		return reward;
	}

	private void UpdateQValue(AgentState state, BaseAction action, float reward, AgentState newState)
	{
		AgentStateActionPair key = new AgentStateActionPair { state = state, action = action };
		// Get the current Q-value
		float currentQ = qTable.ContainsKey(key) ? qTable[key] : qTableInitValue;
		// Get the max Q-value for the new state
		float maxQ = GetMaxQValue(newState);
		// Update Q-value using the Q-learning formula
		float newQ = currentQ + learningRate * (reward + discountFactor * maxQ - currentQ);
		// Store the new Q-value
		qTable[key] = newQ;
	}

	private float GetMaxQValue(AgentState state)
	{
		float maxQ = float.MinValue;

		foreach (BaseAction action in availableActions)
		{
			AgentStateActionPair key = new AgentStateActionPair { state = state, action = action };
			float q = qTable.ContainsKey(key) ? qTable[key] : qTableInitValue;
			if (q > maxQ)
				maxQ = q;
		}

		return maxQ == float.MinValue ? 0f : maxQ;
	}
}
