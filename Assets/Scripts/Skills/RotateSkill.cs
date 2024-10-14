using UnityEngine;

public class RotationSkill : BaseSkill
{
	public float energyCostPerRotation = 1f;
	public float rotationSpeed = 50f;
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
			float rotation = energy / energyCostPerRotation;
			// Spend energy to rotate
			transform.Rotate(Vector3.forward, direction * rotation * rotationSpeed * Time.deltaTime);
		}
	}

	public override void Execute()
	{
		Rotate(1f, 1f);
	}
}