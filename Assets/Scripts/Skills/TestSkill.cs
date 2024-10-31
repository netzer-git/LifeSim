using UnityEngine;

public class TestSkill : BaseSkill
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
		float absoluteAngle = RelativeToAbsoluteAngle(-45f);
		//Debug.Log("Executing TestSkill at angle: " + absoluteAngle);
	}

	public float RelativeToAbsoluteAngle(float relativeAngle)
	{
		// Ensure the relative angle is within the range of 0 to 360 degrees
		relativeAngle = Mathf.Repeat(relativeAngle, 360f);

		// Calculate the absolute angle by adding the object's current rotation
		float absoluteAngle = transform.eulerAngles.z + relativeAngle;

		// Normalize the absolute angle to be within the range of 0 to 360 degrees
		absoluteAngle = Mathf.Repeat(absoluteAngle, 360f);
		absoluteAngle = (absoluteAngle) % 360;

		return absoluteAngle;
	}
}