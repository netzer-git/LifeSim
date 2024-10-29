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
		// Rotate around Z-axis (Vector3.forward) in the correct direction
		transform.Rotate(0, 0, -direction * rotationSpeed * Time.deltaTime);
	}

	public override void Execute()
	{
		Rotate(1f); // Positive value rotates the agent clockwise
	}
}
