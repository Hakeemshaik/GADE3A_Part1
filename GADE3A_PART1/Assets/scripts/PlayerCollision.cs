using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player collided with a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Stop the player's movement by setting velocity to zero
            rb.velocity = Vector3.zero;
            // Alternatively, you can add other collision handling logic here
        }
        if (collision.gameObject.CompareTag("AI"))
        {
            // Stop the player's movement by setting velocity to zero
            rb.velocity = Vector3.zero;
            // Alternatively, you can add other collision handling logic here
        }
    }
}
