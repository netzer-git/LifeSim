using UnityEngine;

public class EatAction : BaseAction
{
	private AgentController agentController;

	private void Start()
	{
		agentController = GetComponent<AgentController>();
	}

	public override bool CanExecute(AgentState agentState)
	{
		// Check if the agent is hungry and near food
		return agentState.hungerLevel != HungerLevel.Low && agentState.seesFood;
	}

	public override void Execute()
	{
		// Implement the logic for eating
		// For example, reduce hunger and consume food
		agentController.currentHunger = 0f;
		// Assume food object is handled elsewhere
	}
}
