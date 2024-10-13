using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
	public GameObject agentPrefab;
	public int agentCount = 10;
	public float spawnRadius = 10f;

	private void Start()
	{
		for (int i = 0; i < agentCount; i++)
		{
			Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius;
			GameObject agentObject = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);

			// Add the Agent component to the agentObject if it's not already there
			Agent agent = agentObject.GetComponent<Agent>();
			if (agent == null)
			{
				agent = agentObject.AddComponent<Agent>();
			}

			// Add the MoveTowardMouseSkill component to the agentObject
			MoveTowardMouseSkill moveSkill = agentObject.AddComponent<MoveTowardMouseSkill>();
			agent.AddSkill(moveSkill);

			// Add other skills as needed
			// For example:
			// FieldOfViewSkill fieldOfViewSkill = agentObject.AddComponent<FieldOfViewSkill>();
			// agent.AddSkill(fieldOfViewSkill);
		}
	}
}