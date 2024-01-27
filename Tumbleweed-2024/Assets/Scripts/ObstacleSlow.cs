using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSlow : MonoBehaviour
{
    // Damping factor to slow down the velocity of the player
    public float dampingFactor = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        // Only care about the player bouncing off of us
        if (other.tag != "Player")
            return;

        // Get the rigidbody of the tumbleweed
        Rigidbody tumbleRb = other.gameObject.GetComponent<Rigidbody>();

        // Ensure the rigidbody exists
        if (tumbleRb != null)
        {
            // Dampen the velocity of the player by multiplying it with the damping factor
            tumbleRb.velocity *= dampingFactor;
        }
    }
}
