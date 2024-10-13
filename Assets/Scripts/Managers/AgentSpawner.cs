using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
	public GameObject agentPrefab;
	public int agentCount = 10;
	public float spawnRadius = 10f;
	public LayerMask targetMask;

	private void Start()
	{
		for (int i = 0; i < agentCount; i++)
		{
			Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius;
			CreateAgent(spawnPosition, i);
		}
	}

	public void CreateAgent(Vector2 spawnPosition, float speed)
	{
		GameObject agentObject = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);

		// Assign a unique name to the agent
		agentObject.name = "Diti" + (speed + 1);

		// Add the Agent component to the agentObject if it's not already there
		Agent agent = agentObject.GetComponent<Agent>();
		if (agent == null)
		{
			agent = agentObject.AddComponent<Agent>();
		}

		// Add the MoveTowardMouseSkill component to the agentObject
		MoveTowardMouseSkill moveSkill = agentObject.AddComponent<MoveTowardMouseSkill>();
		moveSkill.Initialize(speed);
		agent.AddSkill(moveSkill);

		// Add the FieldOfViewSkill component to the agentObject and initialize it
		LookSkill lookSkill = agentObject.AddComponent<LookSkill>();
		lookSkill.Initialize(3f, targetMask); // Set the view radius and target mask
		agent.AddSkill(lookSkill);

		// Add other skills as needed
		// For example:
		// FieldOfViewSkill fieldOfViewSkill = agentObject.AddComponent<FieldOfViewSkill>();
		// agent.AddSkill(fieldOfViewSkill);
	}
}