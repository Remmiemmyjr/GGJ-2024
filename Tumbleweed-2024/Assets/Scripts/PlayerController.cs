using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 currMoveVec;
    Coroutine? reloadCo;

    public Transform Camera;

    public int maxBullets = 6;
    [HideInInspector]
    public int bullets;

    float dirVal;
    float speed;

    public float reloadTime = 2.0f;

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
    float jumpCoolDown = 1f;
    float lastJumpTime = -1f;

    bool isSpeedBoosting;
    bool canSpeedBoost;
    bool canReload;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = accelerationRate;
        currTime = speedBoostTime;
        canSpeedBoost = true;
        bullets = maxBullets;
        canReload = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }


    private void Update()
    {
        if (isSpeedBoosting)
        {
            if (currTime > 0)
            {
                currTime -= Time.deltaTime;
            }
            else if (currTime <= 0)
            {
                isSpeedBoosting = false;
                canSpeedBoost = true;
                currTime = speedBoostTime;
            }
        }

        if (bullets <= 0 && canReload)
        {
            canReload = false;
            StartCoroutine(ReloadGun());
        }
    }

    public void OnMovementInput(InputAction.CallbackContext ctx)
    {
        dirVal = ctx.ReadValue<float>();
    }

    public void OnSpeedBoostInput(InputAction.CallbackContext ctx)
    {
        if(bullets <= 0) return;
        if (Time.realtimeSinceStartup - lastJumpTime < jumpCoolDown) return;


        if (currMoveVec != Vector3.zero && canSpeedBoost && bullets > 0)
        {
            canSpeedBoost = false;
            isSpeedBoosting = true;
            rb.velocity = new Vector3(rb.velocity.x * speedBoost, rb.velocity.y, rb.velocity.z * speedBoost);

            lastJumpTime = Time.realtimeSinceStartup;
            
            // BULLETS
            bullets -= 1;
            Debug.Log(bullets);
        }
    }

    public void OnJumpBoostInput(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled || bullets <= 0) return;

        if (ctx.performed && bullets > 0)
        {
            Vector3 jumpVec = new Vector3(rb.velocity.x, jumpBoostForce, rb.velocity.z);
            rb.velocity = jumpVec;

            // BULLETS
            bullets -= 1;
            Debug.Log(bullets);
        }
    }

    void MovePlayer()
    {
        currMoveVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce((Camera.forward * dirVal) * speed);
        if(currMoveVec.magnitude > maxSpeed && !isSpeedBoosting)
        {
            currMoveVec = currMoveVec.normalized * maxSpeed;
            Vector3 newVel = new Vector3(currMoveVec.x, rb.velocity.y, currMoveVec.z);
            rb.velocity = newVel;
        }
    }

    IEnumerator ReloadGun()
    {
        //Animation
        yield return new WaitForSeconds(reloadTime);
        bullets = maxBullets;
        Debug.Log("Reloaded");
        Debug.Log(bullets);
        canReload = true;
    }
}
