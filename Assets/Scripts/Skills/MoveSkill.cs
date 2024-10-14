using UnityEngine;

public class MoveSkill : BaseSkill
{
	public float energyCostPerMove = 1f;
	public float moveSpeed = 5f;
	public SkillActivationType activationType = SkillActivationType.Active;


	public void Initialize(float energyCost, float speed)
	{
		energyCostPerMove = energyCost;
		moveSpeed = speed;
	}

	public void MoveForward(float energy)
	{
		if (energy >= energyCostPerMove)
		{
			float movement = energy / energyCostPerMove;
			// Spend energy to move
			transform.position += transform.up * moveSpeed * movement * Time.deltaTime;
		}
	}

	public override void Execute()
	{
		MoveForward(1f);
	}
}