using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // Assign the ball's transform to this in the Unity editor
    public float rotationSpeed = 5.0f;
    float rotDir;
    CameraFollow camFollow;

    public void OnRotateInput(InputAction.CallbackContext ctx)
    {
        rotDir = ctx.ReadValue<float>();
        camFollow = GetComponent<CameraFollow>();
    }

    void Update()
    {
        // Rotate the pivot around the ball (Y-axis)
        transform.RotateAround((target.position + camFollow.offset), Vector3.up, (rotationSpeed * rotDir) * Time.deltaTime);
    }
}
