using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFeatures : MonoBehaviour
{
    public float health = 30f;

	public void Initialize(float health)
	{
		this.health = health;
	}
}