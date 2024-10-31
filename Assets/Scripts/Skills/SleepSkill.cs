using System.ComponentModel.Design;
using UnityEngine;

public class SleepSkill : BaseSkill
{
    public SkillActivationType activationType = SkillActivationType.Active;
	
	public override void Execute()
	{
		AgentController agent = this.GetComponent<AgentController>();
		agent.currentEnergy += agent.genome.sleepEnergyGain * Time.deltaTime;
	}

}