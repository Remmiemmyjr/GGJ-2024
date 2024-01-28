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

    private void Start()
    {

    }

    private void Update()
    {
        transform.position = target.position + offset;
        transform.rotation = cmvCam.transform.rotation;
    }
}
