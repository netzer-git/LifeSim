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
    public float currentEnergy;
	public float currentHunger = 0f;

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
		currentHunger += genome.hungerIncreaseRate * Time.deltaTime;
		currentEnergy -= genome.idleEnergyConsumption * Time.deltaTime;
	}

	public void Initialize(GrassGenome grassGenome)
	{
		// FIXME: make sure it's not copied (here it's after mutation)
		this.genome = grassGenome;
		this.currentHealth = this.genome.health;
		this.currentEnergy = this.genome.energy;
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
