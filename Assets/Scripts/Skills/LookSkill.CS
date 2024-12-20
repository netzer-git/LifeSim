﻿using System.Collections.Generic;
using UnityEngine;

public class LookSkill : BaseSkill
{
	public float sightRadius = 2f;
	public float viewAngle = 90f; // Field of view angle
	public LayerMask targetMask;
	public List<DetectedObject> detectedObjects = new List<DetectedObject>();

	public void Initialize(AgentController agent, LayerMask mask)
	{
		sightRadius = agent.genome.sightRadius;
		targetMask = mask;
	}

	private void Update()
	{
		// TODO: Execute by coroutine?
		Execute();
	}

	public override void Execute()
	{
		detectedObjects.Clear();

		Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, sightRadius, targetMask);

		foreach (var target in targetsInViewRadius)
		{
			if (gameObject == target.gameObject)
			{
				continue; // Skip self
			}

			Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
			float angleToTarget = Vector2.SignedAngle(transform.up, directionToTarget);

			if (angleToTarget >= -viewAngle / 2 && angleToTarget <= viewAngle / 2)
			{
				detectedObjects.Add(new DetectedObject(target.gameObject, angleToTarget));
			}

		}
	}
	private void OnDrawGizmos()
	{
		// Draw view radius
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, sightRadius);

		// Draw field of view lines using Quaternion rotations
		Vector3 leftBoundary = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.up;
		Vector3 rightBoundary = Quaternion.Euler(0, 0, viewAngle / 2) * transform.up;

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + leftBoundary * sightRadius);
		Gizmos.DrawLine(transform.position, transform.position + rightBoundary * sightRadius);

		// Optionally draw lines to detected objects
		Gizmos.color = Color.red;
		foreach (var detected in detectedObjects)
		{
			Gizmos.DrawLine(transform.position, detected.Object.transform.position);
		}
	}

}

public class DetectedObject
{
	public GameObject Object { get; private set; }
	public float Angle { get; private set; }

	public DetectedObject(GameObject obj, float angle)
	{
		Object = obj;
		Angle = angle;
	}
}
