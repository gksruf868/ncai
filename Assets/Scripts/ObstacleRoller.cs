using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObstacleRoller : MonoBehaviour
{
    [SerializeField] private float baseAcceleration = 0.5f;
    [SerializeField] private float accelerationGrowthRate = 0.05f;
    [SerializeField] private float maxHorizontalSpeed = 15f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude < 0.1f || horizontalVelocity.magnitude >= maxHorizontalSpeed) return;

        float elapsed = GameManager.Instance != null ? GameManager.Instance.SurvivalTime : 0f;
        float acceleration = baseAcceleration + elapsed * accelerationGrowthRate;

        rb.AddForce(horizontalVelocity.normalized * acceleration, ForceMode.Acceleration);
    }
}
