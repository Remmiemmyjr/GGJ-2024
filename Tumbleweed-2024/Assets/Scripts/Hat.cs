using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    public GameObject hat;
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
        Instantiate(hat, target.position + offset, Quaternion.identity);
    }

    private void Update()
    {
        transform.position = target.position + offset;
    }
}
