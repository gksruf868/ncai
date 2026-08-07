using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObstacleRoller : MonoBehaviour
{
    [SerializeField] private float baseAcceleration = 0.5f;
    [SerializeField] private float accelerationGrowthRate = 0.05f;
    [SerializeField] private float maxHorizontalSpeed = 15f;

    private Rigidbody rb;
    private float speedMultiplier = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    private void FixedUpdate()
    {
        float effectiveMaxSpeed = maxHorizontalSpeed * speedMultiplier;
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude < 0.1f || horizontalVelocity.magnitude >= effectiveMaxSpeed) return;

        float elapsed = GameManager.Instance != null ? GameManager.Instance.SurvivalTime : 0f;
        float acceleration = (baseAcceleration + elapsed * accelerationGrowthRate) * speedMultiplier;

        rb.AddForce(horizontalVelocity.normalized * acceleration, ForceMode.Acceleration);
    }
}
