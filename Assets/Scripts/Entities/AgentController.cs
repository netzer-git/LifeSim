using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AgentController : MonoBehaviour
{
	public List<BaseSkill> skills = new List<BaseSkill>();
	public GrassGenome genome; // genome - specific for grass agent
	public float currentHealth;
    public float currentEnergy; // 0 -> 100
	public float currentHunger = 0f; // 0 -> 1

	private AgentMind agentMind;
	private float decisionMakingCoroutineInterval = 1f;

	private void Start()
	{
		skills.AddRange(GetComponents<BaseSkill>());
		agentMind = GetComponent<AgentMind>();
		StartCoroutine(DecisionMakingCoroutine());
	}

	private void Update()
	{
		// TODO: for debug
		if (Input.GetMouseButton(0))
		{
			ExecuteSkills();
		}

		// Energy decreases and hunger increase.
		currentHunger += genome.hungerIncreaseRate * Time.deltaTime;
		currentEnergy -= genome.idleEnergyConsumption * Time.deltaTime;

		// Execute the action
		ExecuteAction(agentMind.currentAction);
	}

	public void Initialize(GrassGenome grassGenome)
	{
		// FIXME: make sure it's not copied (here it's after mutation)
		genome = grassGenome;
		currentHealth = this.genome.health;
		currentEnergy = this.genome.energy;
	}

	private IEnumerator DecisionMakingCoroutine()
	{
		while (true)
		{
			// Decision-making logic
			agentMind.DecideAction(this);

			// Wait for a fixed interval
			yield return new WaitForSeconds(this.decisionMakingCoroutineInterval);
		}
	}

	private void ExecuteAction(AgentAction action)
	{
		MoveSkill moveSkill = GetComponent<MoveSkill>();
		switch (action)
		{
			case AgentAction.Wander:
				moveSkill.Wander();
				break;
			case AgentAction.SeekFood:
				// Move towards the nearest food
				Vector3? foodPosition = FindNearestObjectPosition("Food");
				if (foodPosition.HasValue)
				{
					moveSkill.MoveTowards(foodPosition.Value);
				}
				else
				{
					moveSkill.Wander();
				}
				break;
			case AgentAction.Flee:
				// Move away from the nearest predator
				Vector3? predatorPosition = FindNearestObjectPosition("Predator");
				if (predatorPosition.HasValue)
				{
					Vector3 directionAway = transform.position - predatorPosition.Value;
					Vector3 fleePosition = transform.position + directionAway.normalized * 2f;
					moveSkill.MoveTowards(fleePosition);
				}
				else
				{
					moveSkill.Wander();
				}
				break;
			case AgentAction.Sleep:
				StartCoroutine(SleepRoutine());
				break;
				// Add other actions
		}
	}

	private IEnumerator SleepRoutine()
	{
		// Implement sleeping behavior
		// For simplicity, we'll just wait and regain energy
		float sleepDuration = 5f; // Adjust as needed
		yield return new WaitForSeconds(sleepDuration);

		// Regain energy
		currentEnergy = genome.energy; // Restore to max energy
	}

	public void AddSkill(BaseSkill skill)
	{
		skills.Add(skill);
		skill.transform.SetParent(transform);
	}

	public void RemoveSkill(BaseSkill skill)
	{
		skills.Remove(skill);
		Destroy(skill);
	}

	public void ExecuteSkills()
	{
		foreach (var skill in skills)
		{
			skill.Execute();
		}
	}
}
