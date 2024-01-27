using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public Transform Camera;

    public int maxBullets;
    [HideInInspector]
    public int bullets;

    float dirVal;
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

        if (bullets <= 0)
        {

        }
    }

    public void OnMovementInput(InputAction.CallbackContext ctx)
    {
        dirVal = ctx.ReadValue<float>();
    }

    public void OnSpeedBoostInput(InputAction.CallbackContext ctx)
    {
        if (canBoost)
        {
            isBoosting = true;
            rb.velocity = new Vector3(rb.velocity.x * speedBoost, rb.velocity.y, rb.velocity.z * speedBoost);
            bullets--;
        }
    }

    public void OnJumpBoostInput(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled) return;
        if (ctx.performed)
        {
            Vector3 jumpVec = new Vector3(rb.velocity.x, jumpBoostForce, rb.velocity.z);
            rb.velocity = jumpVec;
            bullets--;
        }
    }

    void MovePlayer()
    {
        Vector3 currMoveVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce((Camera.forward * dirVal) * speed);
        if(currMoveVec.magnitude > maxSpeed && !isBoosting)
        {
            currMoveVec = currMoveVec.normalized * maxSpeed;
            Vector3 newVel = new Vector3(currMoveVec.x, rb.velocity.y, currMoveVec.z);
            rb.velocity = newVel;
        }
    }
}
