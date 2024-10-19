using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AgentController : MonoBehaviour
{
	public List<BaseSkill> skills = new List<BaseSkill>();
	public DitiGenome ditiGenome; // genome
    public float currentHealth;
    public float currentEnergy;

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

		// TODO: add energy and health reduction
	}

	public void Initialize(DitiGenome ditiGenome)
	{
		// FIXME: make sure it's not copied (here it's after mutation)
		this.ditiGenome = ditiGenome;
		this.currentHealth = this.ditiGenome.health;
		this.currentEnergy = this.ditiGenome.energy;
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

public struct DitiGenome
{
	// health and energy
    public float energy; // start and max amount of energy, replenish on eating. everything costs energy
    public float health; // start and max amount of health, replenish on long rests
	public float idleEnergyConsumption; // energy consumed when idle
    // eat
    public float biteStrength; // amount of food for each bite
    public float biteEnergyCost; // energy cost for each bite
    public float nutrientConsumption; // amount of food that turns into energy
    // move
    public float moveSpeed; // movement speed
    public float moveEnergyCost; // energy cost for a move
    // look
    public float sightRadius; // field of view depth
    // rotate
    public float rotationSpeed; // rotation speed
    // birth
    public float birthEnergyCost; // cost in energy for birth
    public float mutationRate; // mutation rate
}