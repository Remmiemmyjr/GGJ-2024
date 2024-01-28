using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool checkPointReached = false;

    // Storage Variables
    Vector3 checkPointPosition;
    float time = 0.0f;

    private void Start()
    {
        checkPointPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore objects other than the player
        if (other.tag != "Player" && checkPointReached)
            return;

        // Snapshot the time at the checkpoint
        time = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>().GetTime();

        // Set the position and time attributes within the player controller
        other.GetComponent<PlayerController>().resetPosition = checkPointPosition - new Vector3(0.0f, (transform.localScale.y / 4.0f), 0.0f);
        other.GetComponent<PlayerController>().timeOnReset = time;
    }
}
