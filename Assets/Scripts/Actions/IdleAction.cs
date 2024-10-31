using UnityEngine;

public class IdleAction : BaseAction
{
	private AgentController agentController;

	private void Start()
	{
		agentController = GetComponent<AgentController>();
	}

	public override bool CanExecute(AgentState agentState)
	{
		// Check if the agent is hungry and near food
		return true;
	}

	public override void Execute()
	{
		return;
	}
}
