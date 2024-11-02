using System.ComponentModel;
using UnityEngine;

public class EatSkill : BaseSkill
{
	public SkillActivationType activationType = SkillActivationType.OnCollide;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (this.IsEdible(collision.gameObject))
		{
			// Implement eating logic here
			Debug.Log($"{gameObject.name} ate {collision.gameObject.name}");

			AgentController agentController = GetComponent<AgentController>();

			if (agentController.currentEnergy < 0f)
			{
				throw new System.Exception("Energy is below 0 while eating");
			}

			// FIXME: make continues - test with food with high health
			agentController.currentSatiety += agentController.genome.biteStrength;
			agentController.currentEnergy -= agentController.genome.biteEnergyCost;

			// collision impact
			FoodFeatures foodObj = collision.gameObject.GetComponent<FoodFeatures>();
			foodObj.health -= agentController.genome.biteStrength;

			if (foodObj.health <= 0)
			{
				Destroy(collision.gameObject);
			}
		}

	}


	public override void Execute()
	{
        // This skill does not use Execute since it's based on collider
    }

	private bool IsEdible(GameObject gameObject)
	{
		return gameObject.CompareTag("Food");
	}
}