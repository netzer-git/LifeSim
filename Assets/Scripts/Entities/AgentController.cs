using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AgentController : MonoBehaviour
{
	public List<BaseSkill> skills = new List<BaseSkill>();
	public GrassGenome genome; // genome - specific for grass agent
	public float currentHealth = 100; // 0 -> health
	public float currentEnergy = 100; // 0 -> 100
	public float currentSatiety = 100; // 0 -> 100

	private void Start()
	{
		skills.AddRange(GetComponents<BaseSkill>());
	}

	private void Update()
	{
		// TODO: for debug
		if (Input.GetMouseButton(0))
		{
			ExecuteSkills();
		}

		// Energy decreases and hunger increase.
		currentSatiety -= genome.satietyDecreaseRate * Time.deltaTime;
		currentEnergy -= genome.idleEnergyConsumption * Time.deltaTime;

		if (currentSatiety <= 0)
		{
			currentSatiety = 0;
			currentHealth -= genome.satietyDecreaseRate * Time.deltaTime;
		}
	}

	public void Initialize(GrassGenome grassGenome)
	{
		// FIXME: make sure it's not copied (here it's after mutation)
		genome = grassGenome;
		currentHealth = this.genome.health;
		currentEnergy = 100;
		currentSatiety = 100;
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
