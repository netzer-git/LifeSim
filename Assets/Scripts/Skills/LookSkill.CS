using System.Collections.Generic;
using UnityEngine;

public class LookSkill : BaseSkill
{
	public float viewRadius = 5f;
	public LayerMask targetMask;
	public List<DetectedObject> detectedObjects = new List<DetectedObject>();
	public SkillActivationType activationType = SkillActivationType.Update;

	public void Initialize(float radius, LayerMask mask)
	{
		viewRadius = radius;
		targetMask = mask;
	}

	private void Update()
	{
		Execute();
	}

	public override void Execute()
	{
		detectedObjects.Clear();
		Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

		foreach (var target in targetsInViewRadius)
		{
			Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
			float angleToTarget = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
			detectedObjects.Add(new DetectedObject(target.gameObject, angleToTarget));
			
			// Debug.Log($"Agent {gameObject.name} detected object: {target.gameObject.name}, Angle: {angleToTarget}");
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