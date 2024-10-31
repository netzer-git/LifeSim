using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFeatures : MonoBehaviour
{
    public float health = 10f;

	public void Initialize(float health)
	{
		this.health = health;
	}

	private void Update()
	{
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}
}