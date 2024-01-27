using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool checkPointReached = false;

    // Storage Variables
    Vector3 checkPointPosition = Vector3.zero;
    Vector3 checkPointVelocity = Vector3.zero;
    Vector3 checkPointAcceleration = Vector3.zero;
    float time = 0.0f;

    private void Start()
    {
        checkPointPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore objects other than the player
        if (other.tag != "Player" && !checkPointReached)
            return;

        // Snapshot the velocity and acceleration on entrance (could do static values as well)


        // Snapshot the time at the checkpoint
        GameObject.FindGameObjectWithTag("Timer");
    }
}