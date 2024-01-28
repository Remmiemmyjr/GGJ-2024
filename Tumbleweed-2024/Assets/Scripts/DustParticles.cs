using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticles : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public CinemachineVirtualCamera cmvCam;


    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.rotation = cmvCam.transform.rotation;
    }
}
