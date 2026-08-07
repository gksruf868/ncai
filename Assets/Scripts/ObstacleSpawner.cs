using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float intervalDecreaseRate = 0.05f;
    [SerializeField] private float spawnHeight = 15f;
    [SerializeField] private float spawnRangeX = 15f;
    [SerializeField] private float spawnRangeZ = 15f;
    [SerializeField] private float minSpawnRadius = 1.5f;
    [SerializeField] private float maxSpawnRadius = 5f;
    [SerializeField] private float spinStrength = 5f;

    private float timer;
    private float elapsedTime;

    private void Start()
    {
        SpawnObstacle();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        elapsedTime += Time.deltaTime;
        timer += Time.deltaTime;

        float currentInterval = Mathf.Max(minSpawnInterval, spawnInterval - elapsedTime * intervalDecreaseRate);
        if (timer >= currentInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        Vector3 center = player != null ? player.position : Vector3.zero;
        Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);

        float x = Mathf.Clamp(center.x + offset.x, -spawnRangeX, spawnRangeX);
        float z = Mathf.Clamp(center.z + offset.y, -spawnRangeZ, spawnRangeZ);
        Vector3 spawnPosition = new Vector3(x, spawnHeight, z);

        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        Rigidbody rb = obstacle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.angularVelocity = Random.insideUnitSphere * spinStrength;
        }
    }
}
