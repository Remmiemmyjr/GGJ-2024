using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector2 dir;
    float accelerationRate = 10.0f;
    float changeDirForce = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    public void OnMovementInput(InputAction.CallbackContext ctx)
    {
        dir = ctx.ReadValue<Vector2>();
    }

    void MovePlayer()
    {
        dir = dir.normalized;
        Vector3 moveVec = new Vector3(dir.x, rb.velocity.y, dir.y);

        //float dotProduct = Vector3.Dot(rb.velocity.normalized, dir);

        rb.AddForce(moveVec * accelerationRate);
    }    
}
