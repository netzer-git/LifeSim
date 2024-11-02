using UnityEngine;

public class SleepAction : BaseAction
{
	private AgentController agentController;
	public bool isCurrentlySleeping = false;

	private void Start()
	{
		agentController = GetComponent<AgentController>();
	}

	private void Update()
	{
		if (isCurrentlySleeping)
		{
			agentController.currentEnergy += agentController.genome.sleepEnergyGain * Time.deltaTime;
			if (agentController.currentHealth < agentController.genome.health)
			{
				agentController.currentHealth += agentController.genome.sleepHealthGain * Time.deltaTime;
			}
		}
	}

	public override bool CanExecute(AgentState agentState)
	{
		// Check if the agent is hungry and near food
		return true;
	}

	public override void Execute()
	{
		isCurrentlySleeping = true;
	}

	public override void Finish()
	{
		isCurrentlySleeping = false;
	}
}
