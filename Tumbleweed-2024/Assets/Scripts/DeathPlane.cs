using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Ignore objects other than the player
        if (other.tag != "Player")
            return;

        // TODO: Call the player reset function here

    }
}
