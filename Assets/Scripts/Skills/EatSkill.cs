using UnityEngine;

public class EatSkill : BaseSkill
{
	public SkillActivationType activationType = SkillActivationType.OnCollide;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Food"))
		{
			// Implement eating logic here
			Debug.Log($"{gameObject.name} ate {collision.gameObject.name}");

			Destroy(collision.gameObject); // Remove the edible object
		}

	}


	public override void Execute()
	{
        // This skill does not use Execute since it's based on collider
    }
}