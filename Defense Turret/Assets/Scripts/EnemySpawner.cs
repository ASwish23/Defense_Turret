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

    [Header("Meteor Prefabs (new)")]
    public GameObject enemySmallPrefab;
    public GameObject enemyBigPrefab;

    [Header("Wave Settings")]
    public float timeBetweenWaves = 5f;
    public Wave[] waves;

    [Header("Spawn Area")]
    public Transform[] spawnPoints; // deja folosit

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

                yield return new WaitForSeconds(1f / currentWave.spawnRate);
            }

            Debug.Log($"Wave finished! Resting for {timeBetweenWaves} seconds.");
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
        }

        Debug.Log("All waves complete!");
    }

    void SpawnRandomEnemy()
    {
        if (spawnPoints.Length == 0) return;

        // Alege un spawn point random
        int pointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[pointIndex];

        // Alege tipul entity (weighted)
        int diceRoll = Random.Range(0, 100);
        GameObject selectedPrefab = null;

        if (diceRoll < 40) // 40% weak
            selectedPrefab = weakPrefab;
        else if (diceRoll < 60) // 20% medium
            selectedPrefab = mediumPrefab;
        else if (diceRoll < 70) // 10% strong
            selectedPrefab = strongPrefab;
        else if (diceRoll < 90) // 20% EnemySmall
            selectedPrefab = enemySmallPrefab;
        else // 10% EnemyBig
            selectedPrefab = enemyBigPrefab;

        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log($"Spawned {selectedPrefab.name} at {spawnPoint.position}");
        }
    }
}
