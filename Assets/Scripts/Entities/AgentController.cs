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
		if (agentMind.currentAction.CanExecute(agentMind.currentState))
		{
			agentMind.currentAction.Execute();
		}
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
