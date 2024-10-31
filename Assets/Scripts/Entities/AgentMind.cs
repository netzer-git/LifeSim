using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AgentMind : MonoBehaviour
{
	// RL Components
	private Dictionary<AgentStateActionPair, float> qTable = new Dictionary<AgentStateActionPair, float>();
	private float reward;
	private AgentState currentState;
	private AgentAction currentAction;

	// Learning parameters
	public float learningRate = 0.1f;
	public float discountFactor = 0.9f; // value close to 1 encourages long-term rewards.
	public float explorationRate = 0.2f; // For epsilon-greedy exploration

	void OnDrawGizmos()
	{
		// Define the position above or next to the agent
		Vector3 labelPosition = transform.position + Vector3.up * 1.5f;
		Handles.Label(labelPosition, $"Action: {currentAction}");
	}

	public void DecideAction()
	{
		AgentController agentController = GetComponent<AgentController>();
		// Observe the current state
		AgentState newState = GetCurrentState(agentController);
		// Receive reward from the last action
		reward = GetReward(currentState, currentAction, newState);
		// Update Q-value
		UpdateQValue(currentState, currentAction, reward, newState);
		// Decide next action
		currentState = newState;
		currentAction = SelectAction(currentState);
	}

	public AgentState GetCurrentState(AgentController agentController)
	{
		AgentState state = new AgentState();
		// Discretize hunger level
		if (agentController.currentHunger < 0.3f)
			state.hungerLevel = HungerLevel.Low;
		else if (agentController.currentHunger < 0.7f)
			state.hungerLevel = HungerLevel.Medium;
		else
			state.hungerLevel = HungerLevel.High;

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

	public AgentAction SelectAction(AgentState state)
	{
		List<AgentAction> actions = GetAvailableActions(state);
		// Epsilon-greedy strategy for exploration
		if (Random.value < explorationRate)
		{
			// Random action (exploration)
			return actions[Random.Range(0, actions.Count)];
		}
		else
		{
			// Best action based on Q-values (exploitation)
			AgentAction bestAction = actions[0];
			float maxQ = float.MinValue;
			foreach (AgentAction action in actions)
			{
				AgentStateActionPair key = new AgentStateActionPair { state = state, action = action };
				float q = qTable.ContainsKey(key) ? qTable[key] : 0f;

				if (q > maxQ)
				{
					maxQ = q;
					bestAction = action;
				}
			}

			return bestAction;
		}
	}


	private float GetReward(AgentState previousState, AgentAction action, AgentState newState)
	{
		// FIXME: reward multipliers are arbitrary and given from o1
		float reward = 0f;

		// Positive reward for decreasing hunger
		if (newState.hungerLevel < previousState.hungerLevel)
			reward += (previousState.hungerLevel - newState.hungerLevel) * 10f;

		// Negative reward for increasing hunger
		if (newState.hungerLevel > previousState.hungerLevel)
			reward -= (newState.hungerLevel - previousState.hungerLevel) * 5f;

		// Negative reward for low energy
		if (newState.energyLevel == EnergyLevel.Low)
			reward -= 1f;

		// Negative reward for being near a predator
		if (newState.detectedObjectsTypes.Contains(DetectedObjectType.Predator))
			reward -= 5f;

		// Positive reward for gaining energy (after sleeping)
		if (newState.energyLevel > previousState.energyLevel)
			reward += (newState.energyLevel - previousState.energyLevel) * 2f;

		return reward;
	}

	private void UpdateQValue(AgentState state, AgentAction action, float reward, AgentState newState)
	{
		AgentStateActionPair key = new AgentStateActionPair { state = state, action = action };
		// Get the current Q-value
		float currentQ = qTable.ContainsKey(key) ? qTable[key] : 0f;
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

		foreach (AgentAction action in GetAvailableActions(state))
		{
			AgentStateActionPair key = new AgentStateActionPair { state = state, action = action };
			float q = qTable.ContainsKey(key) ? qTable[key] : 0f;
			if (q > maxQ)
				maxQ = q;
		}

		return maxQ == float.MinValue ? 0f : maxQ;
	}

	private List<AgentAction> GetAvailableActions(AgentState state)
	{
		List<AgentAction> actions = new List<AgentAction>();

		// The agent can always wander
		actions.Add(AgentAction.Wander);
		// actions.Add(AgentAction.SeekMate);
		actions.Add(AgentAction.SeekShelter);

		// If hungry, seeking food is an option
		if (state.hungerLevel != HungerLevel.Low)
		{
			actions.Add(AgentAction.SeekFood);
		}

		// If energy is low, sleeping is an option
		if (state.energyLevel != EnergyLevel.High)
		{
			actions.Add(AgentAction.Sleep);
		}

		// If predator is nearby, fleeing is an option
		if (state.detectedObjectsTypes.Contains(DetectedObjectType.Predator))
		{
			actions.Add(AgentAction.Flee);
		}

		return actions;
	}
}
