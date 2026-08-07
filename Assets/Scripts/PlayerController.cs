using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float boundaryLimit = 19.5f;

    private bool isGameOver = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isGameOver || GameManager.Instance == null || !GameManager.Instance.HasStarted)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Keyboard keyboard = Keyboard.current;
        float horizontal = 0f;
        float vertical = 0f;

        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed) horizontal -= 1f;
            if (keyboard.dKey.isPressed) horizontal += 1f;
            if (keyboard.sKey.isPressed) vertical -= 1f;
            if (keyboard.wKey.isPressed) vertical += 1f;
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveVelocity = direction * moveSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

        Vector3 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -boundaryLimit, boundaryLimit);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -boundaryLimit, boundaryLimit);
        rb.position = clampedPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;
            GameManager.Instance.GameOver();
            GetComponent<Renderer>().material.SetColor("_BaseColor", Color.black);
        }
    }
}
