using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wave
{
    public string waveName = "Wave 1";
    public int enemyCount = 10;      // Total enemies in this wave
    public float spawnRate = 1.0f;   // Enemies per second
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject weakPrefab;
    public GameObject mediumPrefab;
    public GameObject strongPrefab;

    [Header("Wave Settings")]
    public float timeBetweenWaves = 5f;
    public Wave[] waves;

    [Header("Spawn Area")]
    public Transform[] spawnPoints;     // The 6 spawn points you created
    public float timeBetweenSpawns = 2.0f; // Default to 2 seconds

    private int currentWaveIndex = 0;
    private bool spawningActive = true;

    void Start()
    {
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log($"Starting {currentWave.waveName}");

            for (int i = 0; i < currentWave.enemyCount; i++)
            {
                if (!spawningActive) yield break;

                SpawnRandomEnemy();

                yield return new WaitForSeconds(timeBetweenSpawns); // Waits for your variable amount
            }

            Debug.Log($"Wave finished! Resting for {timeBetweenWaves} seconds.");
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
        }
        Debug.Log("All waves complete!");
    }

    void SpawnRandomEnemy()
    {
        // 1. Safety Check: Make sure prefabs are assigned
        if (weakPrefab == null || mediumPrefab == null || strongPrefab == null)
        {
            Debug.LogError("Please assign all Enemy Prefabs in the Inspector!");
            return;
        }

        // 2. Pick a Random Spawn Point
        if (spawnPoints.Length == 0) return;
        int pointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[pointIndex];

        // 3. Pick a Random Enemy Type (Weighted Probability)
        // 60% Weak, 30% Medium, 10% Strong
        int diceRoll = Random.Range(0, 100);
        GameObject selectedPrefab;

        if (diceRoll < 60)
        {
            selectedPrefab = weakPrefab;
        }
        else if (diceRoll < 90)
        {
            selectedPrefab = mediumPrefab;
        }
        else
        {
            selectedPrefab = strongPrefab;
        }

        // 4. Spawn the Enemy
        Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        // Note: We don't need to manually set the Turret reference anymore.
        // The 'Enemy' script on the prefab automatically finds the Turret by Tag in Start().
    }
}