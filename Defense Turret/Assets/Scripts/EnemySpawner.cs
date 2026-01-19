using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wave
{
    public string waveName = "Wave 1";
    public int enemyCount = 10;
    public float spawnRate = 1.0f;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject weakPrefab;
    public GameObject mediumPrefab;
    public GameObject strongPrefab;

    [Header("Meteor Prefabs")]
    public GameObject enemySmallPrefab;
    public GameObject enemyBigPrefab;

    [Header("Wave Settings")]
    public float timeBetweenWaves = 5f;
    public Wave[] waves;

    [Header("Spawn Area")]
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;

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
                SpawnRandomEnemy();
                yield return new WaitForSeconds(1f / currentWave.spawnRate);
            }

            Debug.Log("Wave finished! Resting...");
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
        }

        Debug.Log("All waves complete! You Win!");
    }

    void SpawnRandomEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int pointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[pointIndex];

        // Alege tipul entity (weighted) - Poti ajusta procentele cum vrei
        int diceRoll = Random.Range(0, 100);
        GameObject selectedPrefab = null;

        if (diceRoll < 40) selectedPrefab = weakPrefab;       // 40%
        else if (diceRoll < 60) selectedPrefab = mediumPrefab; // 20%
        else if (diceRoll < 70) selectedPrefab = strongPrefab; // 10%
        else if (diceRoll < 90) selectedPrefab = enemySmallPrefab; // 20%
        else selectedPrefab = enemyBigPrefab;                  // 10%

        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}