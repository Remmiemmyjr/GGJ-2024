using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 currMoveVec;
    Coroutine reloadCo;

    public Animator reloadUIAnimator;

    public CinemachineVirtualCamera cmvCam;
    float rotDir;
    public float rotSpeed = 2.0f;
    public float FOV = 55f;

    public int maxBullets = 6;
    [HideInInspector]
    public int bullets;

    float dirVal;
    float speed;
    float normalFOV;

    public float reloadTime = 3.0f;

    [SerializeField]
    float accelerationRate = 100.0f;
    [SerializeField]
    float maxSpeed = 20.0f;
    public float smoothDamp = 0.95f;

    [SerializeField]
    float speedBoost = 50.0f;
    [SerializeField]
    float speedBoostTime = 1.0f;
    float currTime;

    [SerializeField]
    float jumpBoostForce = 20.0f;
    float jumpCoolDown = 0.25f;
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
        normalFOV = cmvCam.m_Lens.FieldOfView;
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
                StartCoroutine(ChangeFOV(FOV, normalFOV, 0.5f));
            }
        }

        if (bullets <= 0 && canReload)
        {
            canReload = false;
            StartCoroutine(ReloadGun());
        }
    }


    private void LateUpdate()
    {
        RotateCamera();
    }


    public void OnMovementInput(InputAction.CallbackContext ctx)
    {
        dirVal = ctx.ReadValue<float>();
    }


    public void OnSpeedBoostInput(InputAction.CallbackContext ctx)
    {
        if (bullets <= 0) return;
        
        if (currMoveVec != Vector3.zero && canSpeedBoost && bullets > 0)
        {
            canSpeedBoost = false;
            isSpeedBoosting = true;
            Vector3 forwardVec = cmvCam.transform.forward.normalized;
            rb.velocity = new Vector3(forwardVec.x * speedBoost, rb.velocity.y, forwardVec.z * speedBoost);
            StartCoroutine(ChangeFOV(normalFOV, FOV, 0.2f));
            
            // BULLETS
            bullets -= 1;
            Debug.Log(bullets);

            reloadUIAnimator.SetInteger("RevolverAmmo", bullets);
        }
    }


    public void OnJumpBoostInput(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled || bullets <= 0) return;
        if (Time.realtimeSinceStartup - lastJumpTime < jumpCoolDown) return;

        if (ctx.performed && bullets > 0)
        {
            Vector3 jumpVec = new Vector3(rb.velocity.x, jumpBoostForce, rb.velocity.z);
            rb.velocity = jumpVec;

            lastJumpTime = Time.realtimeSinceStartup;


            // BULLETS
            bullets -= 1;
            Debug.Log(bullets);

            reloadUIAnimator.SetInteger("RevolverAmmo", bullets);
        }
    }


    public void OnRotateInput(InputAction.CallbackContext ctx)
    {
        rotDir = ctx.ReadValue<float>();
    }


    void MovePlayer()
    {
        currMoveVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce((cmvCam.transform.forward * dirVal) * speed);

        // Apply extra turning force while moving
        if(dirVal != 0)
            rb.AddForce((cmvCam.transform.right * rotDir) * speed);

        if (currMoveVec.magnitude > maxSpeed)
        {
            currMoveVec = currMoveVec.normalized * maxSpeed;
            Vector3 newVel = new Vector3(currMoveVec.x, rb.velocity.y, currMoveVec.z);

            if (!isSpeedBoosting)
            { 
                rb.velocity = newVel; 
            }

            if(isSpeedBoosting)
            {
                Mathf.Lerp(rb.velocity.x, newVel.x, 0.5f);
                Mathf.Lerp(rb.velocity.z, newVel.z, 0.5f);
            }
        }

        // If the player is not giving move input or falling, slow it to a stop
        if(dirVal == 0 && Mathf.Abs(rb.velocity.y) <= 0.25f )
        {
            rb.velocity *= smoothDamp;
            if (rb.velocity.sqrMagnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }


    void RotateCamera()
    {
        cmvCam.transform.RotateAround(gameObject.transform.position, Vector3.up, rotDir * rotSpeed);
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

    IEnumerator ChangeFOV(float startFOV, float endFOV, float duration)
    {
        //float startFOV = cmvCam.m_Lens.FieldOfView;
        float time = 0;
        //float duration = 0.25f;
        while (time < duration)
        {
            cmvCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
    }
}
