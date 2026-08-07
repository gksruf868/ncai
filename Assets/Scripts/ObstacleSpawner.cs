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
    [SerializeField] private float fastBallActivationTime = 20f;
    [SerializeField] private int fastBallChanceOutOf = 5;
    [SerializeField] private float fastBallSpeedMultiplier = 1.5f;
    [SerializeField] private float bigBallActivationTime = 40f;
    [SerializeField] private int bigBallChanceOutOf = 7;
    [SerializeField] private float bigBallSizeMultiplier = 1.5f;

    private float timer;
    private float elapsedTime;
    private bool hasSpawnedFirst;

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.HasStarted || GameManager.Instance.IsGameOver) return;

        if (!hasSpawnedFirst)
        {
            hasSpawnedFirst = true;
            SpawnObstacle();
        }

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

        float survivalTime = GameManager.Instance != null ? GameManager.Instance.SurvivalTime : elapsedTime;
        bool isFastBall = survivalTime >= fastBallActivationTime && Random.Range(0, fastBallChanceOutOf) == 0;
        float speedMultiplier = isFastBall ? fastBallSpeedMultiplier : 1f;

        Rigidbody rb = obstacle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.angularVelocity = Random.insideUnitSphere * spinStrength * speedMultiplier;
        }

        ObstacleRoller roller = obstacle.GetComponent<ObstacleRoller>();
        if (roller != null)
        {
            roller.SetSpeedMultiplier(speedMultiplier);
        }

        bool isBigBall = survivalTime >= bigBallActivationTime && Random.Range(0, bigBallChanceOutOf) == 0;
        if (isBigBall)
        {
            obstacle.transform.localScale *= bigBallSizeMultiplier;
        }
    }
}
