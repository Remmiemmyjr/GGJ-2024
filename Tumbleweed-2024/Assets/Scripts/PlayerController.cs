using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector2 dir;
    Vector3 moveVec;

    float speed;

    [SerializeField]
    float accelerationRate = 30.0f;
    [SerializeField]
    float maxSpeed = 10.0f;


    [SerializeField]
    float speedBoost = 50.0f;
    [SerializeField]
    float speedBoostTime = 1.0f;
    float currTime;


    [SerializeField]
    float jumpBoostForce = 3.0f;

    bool isBoosting;
    bool canBoost;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = accelerationRate;
        currTime = speedBoostTime;
        canBoost = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }


    private void Update()
    {
        if (isBoosting)
        {
            canBoost = false;
            if (currTime > 0)
            {
                currTime -= Time.deltaTime;
            }
            else if (currTime <= 0)
            {
                isBoosting = false;
                canBoost = true;
                currTime = speedBoostTime;
            }
        }
    }

    public void OnMovementInput(InputAction.CallbackContext ctx)
    {
        dir = ctx.ReadValue<Vector2>();
    }

    public void OnSpeedBoostInput(InputAction.CallbackContext ctx)
    {
        if (canBoost)
        {
            isBoosting = true;
            rb.velocity = new Vector3(rb.velocity.x * speedBoost, rb.velocity.y, rb.velocity.z * speedBoost);
        }
    }

    public void OnJumpBoostInput(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled) return;
        if (ctx.performed)
        {
            Vector3 jumpVec = new Vector3(rb.velocity.x, jumpBoostForce, rb.velocity.z);
            rb.velocity = jumpVec;
        }
    }

    void MovePlayer()
    {
        dir = dir.normalized;
        moveVec = new Vector3(dir.x, 0, dir.y);

        Vector3 currMoveVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(moveVec.normalized * speed);
        if(currMoveVec.magnitude > maxSpeed && !isBoosting)
        {
            currMoveVec = currMoveVec.normalized * maxSpeed;
            Vector3 newVel = new Vector3(currMoveVec.x, rb.velocity.y, currMoveVec.z);
            rb.velocity = newVel;
        }
    }
}
