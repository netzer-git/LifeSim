using UnityEngine;

public class MoveSkill : BaseSkill
{
	public float energyCostPerMove = 1f;
	public float moveForce = 5f;
	public SkillActivationType activationType = SkillActivationType.Active;


	public void Initialize(float energyCost, float force)
	{
		energyCostPerMove = energyCost;
		moveForce = force;
	}

	public void MoveForward(float energy)
	{
		if (energy >= energyCostPerMove)
		{
			float movement = energy / energyCostPerMove;
			// Spend energy to move
			GetComponent<Rigidbody2D>().AddForce(transform.up * moveForce * movement);
		}
	}

	public override void Execute()
	{
		MoveForward(1f);
	}
}