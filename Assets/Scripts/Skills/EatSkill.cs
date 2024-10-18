using UnityEngine;

public class EatSkill : BaseSkill
{
	public SkillActivationType activationType = SkillActivationType.OnCollide;

	private void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.gameObject.CompareTag("Edible"))
		{
			// Implement eating logic here
			Debug.Log($"{gameObject.name} ate {collider.gameObject.name}");

			Destroy(collider.gameObject); // Remove the edible object
		}
	}

	public override void Execute()
	{
        // This skill does not use Execute since it's based on collider
    }
}