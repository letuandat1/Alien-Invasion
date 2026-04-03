using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    public float spawnFrequency = 1f; // Quái mỗi giây
    private float nextSpawnTime;

    [Header("Tăng tốc theo thời gian")]
    public float frequencyIncreaseInterval = 30f; // mỗi 60 giây
    private float timeSinceStart = 0f;
    private float nextIncreaseTime = 30f;

    [Header("Spawn Area")]
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    void Update()
    {
        timeSinceStart += Time.deltaTime;

        // Tăng tốc độ spawn mỗi 60 giây
        if (timeSinceStart >= nextIncreaseTime)
        {
            spawnFrequency += 0.2f;
            nextIncreaseTime += frequencyIncreaseInterval;
            Debug.Log("Tăng tốc độ spawn: " + spawnFrequency + " quái/giây");
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomEnemy();
            nextSpawnTime = Time.time + (1f / spawnFrequency);
        }
    }

    void SpawnRandomEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[index];

        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

        // Gán cấp chỉ số dựa vào thời gian
        Alien alienScript = enemy.GetComponent<Alien>();
        if (alienScript != null)
        {
            alienScript.ApplyStatLevel(Alien.currentStatLevel);
        }
    }
}
