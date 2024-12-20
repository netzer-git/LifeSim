using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public int startFoodCount = 10;
    public float spawnRadius = 10f;
    public LayerMask targetMask;

    private int foodId = 1;

    private void Start()
    {
        for (int i = 0; i < startFoodCount; i++)
        {
            Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius;
            CreateFood(spawnPosition, 10);
        }
    }

	public void CreateFood(Vector2 spawnPosition, float maxFoodHealth)
    {
        GameObject foodObject = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

        // Assign a unique name to the agent
        foodObject.name = "Food-" + (foodId);
        foodId++;
        float foodHealth = Random.Range(maxFoodHealth / 2, maxFoodHealth);

		FoodFeatures foodFeatures = foodObject.GetComponent<FoodFeatures>();
        if (foodFeatures == null)
        {
            foodFeatures = foodObject.AddComponent<FoodFeatures>();
        }
        foodFeatures.Initialize(foodHealth);
	}
}