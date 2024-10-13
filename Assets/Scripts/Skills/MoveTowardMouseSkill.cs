using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveTowardMouseSkill : BaseSkill
{
	public float moveSpeed = 0f;
	public SkillActivationType skillActivationType = SkillActivationType.Update;
	private Vector2 targetPosition;
	private bool isMoving = false;

	public void Initialize(float speed)
	{
		moveSpeed = speed;
	}

	private void Update()
	{
		Execute();
	}

	public override void Execute()
	{
		if (Input.GetMouseButton(0))
		{
			targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}

		if (isMoving)
		{
			Vector2 currentPosition = transform.position;
			Vector2 moveDirection = (targetPosition - currentPosition).normalized;
			transform.position = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

			if (moveDirection != Vector2.zero)
			{
				float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
			}
		}
	}
}