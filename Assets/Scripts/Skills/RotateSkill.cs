using UnityEngine;

public class RotationSkill : BaseSkill
{
	public float rotationSpeed = 50f;
	public SkillActivationType activationType = SkillActivationType.Active;

	public void Initialize(float speed)
	{
		rotationSpeed = speed;
	}

	public void Rotate(float direction)
	{
		// Spend energy to rotate
		transform.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
	}

	public override void Execute()
	{
		Rotate(1f);
	}
}