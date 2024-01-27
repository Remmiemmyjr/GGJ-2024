using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBounce : MonoBehaviour
{
    // Multiflier for the opposite bounce force
    public float bounceMultiplier = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        // Only care about the player bouncing off of us
        if (collision.gameObject.tag != "Player")
            return;

        // Get the rigidbody of the tumbleweed
        Rigidbody tumbleRb = collision.gameObject.GetComponent<Rigidbody>();

        // Ensure the rigidbody exists
        if (tumbleRb != null)
        {
            // Get the current velocity of the tumbleweed
            Vector3 currentVelocity = tumbleRb.velocity;

            // Calculate the opposite direction of the velocity
            Vector3 oppositeDirection = -currentVelocity.normalized;

            // Apply the bounce force in the opposite direction
            tumbleRb.AddForce(oppositeDirection * bounceMultiplier, ForceMode.Impulse);
        }
    }
}
