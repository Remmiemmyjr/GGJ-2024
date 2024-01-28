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

    public float amplitude = 1.0f; 
    public float frequency = 1.0f;

    private Vector3 initialPosition;


    private void Start()
    {
        initialPosition = transform.position + offset;
    }

    private void Update()
    {
        float yOffset = amplitude * Mathf.Sin(Time.time * frequency);
        //transform.position = target.position + offset;
        transform.position = initialPosition + new Vector3(transform.position.x, yOffset, transform.position.z);
        transform.rotation = cmvCam.transform.rotation;
    }
}
