using System.Collections;
using UnityEngine;

public class PivotSwing : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPosition;

    [Header("Movement Settings")]
    public float distance = 3f; // Total width of the movement
    public float speed = 2f;    // Speed of the wiper effect

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    void FixedUpdate()
    {
        // Calculate the left/right offset using PingPong
        float offset = Mathf.PingPong(Time.time * speed, distance) - (distance / 2f);

        // New target position based on the start position
        Vector3 targetPosition = startPosition + new Vector3(offset, 0f, 0f);

        // Move the Rigidbody safely while preserving physics interactions
        rb.MovePosition(targetPosition);
    }
}
