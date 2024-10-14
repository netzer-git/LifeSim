using UnityEngine;

public class RotationSkill : BaseSkill
{
	public float energyCostPerRotation = 1f;
	public float rotationSpeed = 100f;
	public SkillActivationType activationType = SkillActivationType.Active;

	public void Initialize(float energyCost, float speed)
	{
		energyCostPerRotation = energyCost;
		rotationSpeed = speed;
	}

	public void Rotate(float energy, float direction)
	{
		if (energy >= energyCostPerRotation)
		{
			float movement = energy % energyCostPerRotation;
			// Spend energy to rotate
			GetComponent<Rigidbody2D>().AddTorque(direction * rotationSpeed);
		}
	}

	public override void Execute()
	{
	}
}