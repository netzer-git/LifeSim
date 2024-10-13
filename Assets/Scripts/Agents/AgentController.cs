using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
	public float moveSpeed = 5f;
	private Vector2 targetPosition;
	private bool isMoving = false;

	private void Update()
	{
		// Detect mouse click and set target position
		if (Input.GetMouseButtonDown(0))
		{
			targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			isMoving = true;
		}
		else 
		{
			isMoving = false;
		}

		// Move the agent towards the target position
		if (isMoving)
		{
			Vector2 currentPosition = transform.position;
			Vector2 moveDirection = (targetPosition - currentPosition).normalized;
			transform.position = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

			// Rotate the agent to face the movement direction
			if (moveDirection != Vector2.zero)
			{
				float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust the angle to match the sprite's orientation
			}

			// Check if the agent has reached the target position
			if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
			{
				isMoving = false;
			}
		}
	}
}
