using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
	public GameObject agentPrefab;
	public int agentCount = 1;
	public float spawnRadius = 10f;
	public LayerMask targetMask;

	private void Start()
	{
		for (int i = 0; i < agentCount; i++)
		{
			Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius;
			CreateAgent(spawnPosition, i + 1);
		}
	}

	public void CreateAgent(Vector2 spawnPosition, float speed)
	{
		GameObject agentObject = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);

		// Assign a unique name to the agent
		agentObject.name = "Diti-" + (speed + 1);

        // Add the Agent component to the agentObject if it's not already there
        AgentController agent = agentObject.GetComponent<AgentController>();
		if (agent == null)
		{
			agent = agentObject.AddComponent<AgentController>();
        }

		// Add the MoveTowardMouseSkill component to the agentObject
		MoveTowardMouseSkill mouseMoveSkill = agentObject.AddComponent<MoveTowardMouseSkill>();
		mouseMoveSkill.Initialize(speed);
		agent.AddSkill(mouseMoveSkill);

		// Add the FieldOfViewSkill component to the agentObject and initialize it
		LookSkill lookSkill = agentObject.AddComponent<LookSkill>();
		lookSkill.Initialize(3f, targetMask); // Set the view radius and target mask
		agent.AddSkill(lookSkill);

		// Add the RotateSkill component to the agentObject and initialize it
		//RotationSkill rotateSkill = agentObject.AddComponent<RotationSkill>();
		//rotateSkill.Initialize(1f, 50f); 
		//agent.AddSkill(rotateSkill);

		// Add the MoveSkill component to the agentObject and initialize it
		//MoveSkill moveSkill = agentObject.AddComponent<MoveSkill>();
		//moveSkill.Initialize(1f, 5f);
		//agent.AddSkill(moveSkill);

		// Add the EatSkill component to the agentObject and initialize it
		EatSkill eatSkill = agentObject.AddComponent<EatSkill>();
		agent.AddSkill(eatSkill);

		// Add other skills as needed
		// For example:
		// FieldOfViewSkill fieldOfViewSkill = agentObject.AddComponent<FieldOfViewSkill>();
		// agent.AddSkill(fieldOfViewSkill);
	}
}