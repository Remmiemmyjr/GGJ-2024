using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    public GameObject hat;
    public Transform target;
    public Vector3 offset;
    public CinemachineVirtualCamera cmvCam;

    public float amplitude = 0.1f; 
    public float frequency = 0.1f;

    float yOffset;

    private void Start()
    {
        //initialPosition = transform.position + offset;
    }

    private void Update()
    {
        if (Mathf.Abs(target.gameObject.GetComponent<PlayerController>().currMoveVec.magnitude) > 5f)
        {
            yOffset = amplitude * Mathf.Sin(Time.time * frequency);
        }
        else
        {
            yOffset = 0;
        }

        Vector3 targetPosition = new Vector3(target.position.x, (target.position.y + yOffset) + offset.y, target.position.z);

        // Apply the offset to the hat's position relative to the target
        transform.position = targetPosition;
        transform.rotation = cmvCam.transform.rotation;
    }
}
