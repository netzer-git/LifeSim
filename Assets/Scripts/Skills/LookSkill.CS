using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.Rendering.VirtualTexturing;

public class LookSkill : BaseSkill
{
	public float viewRadius = 2f;
	public float viewAngle = 90f; // Half-circle field of view
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
			if (gameObject.name == target.gameObject.name)
			{
				continue;
			}

			Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
			// Calculate the angle using the Diti's forward direction
			float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

			// Check if the target is within the field of view angle
			if (angleToTarget >= -viewAngle / 2 && angleToTarget <= viewAngle / 2)
			{
				detectedObjects.Add(new DetectedObject(target.gameObject, angleToTarget));
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, viewRadius);

		Vector3 viewAngleA = DirFromAngle(-viewAngle / 2);
		Vector3 viewAngleB = DirFromAngle(viewAngle / 2);

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
		Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
	}

	private Vector3 DirFromAngle(float angleInDegrees)
	{
		angleInDegrees += transform.eulerAngles.z;
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
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