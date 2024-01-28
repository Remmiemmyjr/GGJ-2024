using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [HideInInspector]
    public Vector3 currMoveVec;

    float dirVal;
    float speed;
    float normalFOV;

    [Header("--------VISUALS--------")]
    public Animator reloadUIAnimator;
    public ParticleSystem dustParticles;


    [Header("--------CAMERA---------")]
    public CinemachineVirtualCamera cmvCam;
    float rotDir;
    public float rotSpeed = 2.0f;
    public float FOV = 60f;


    [Header("--------BULLETS--------")]
    public int maxBullets = 6;
    [HideInInspector]
    public int bullets;
    public float reloadTime = 2.5f;


    [Header("-------MOVEMENT-------")]
    [SerializeField]
    float accelerationRate = 100.0f;
    [SerializeField]
    float maxSpeed = 20.0f;
    public float smoothDamp = 0.95f;


    [Header("------SPEED BOOST------")]
    [SerializeField]
    float speedBoost = 50.0f;
    [SerializeField]
    float speedBoostTime = 1.0f;
    float currTime;


    [Header("------JUMP BOOST------")]
    [SerializeField]
    float jumpBoostForce = 20.0f;
    float jumpCoolDown = 0.25f;
    float lastJumpTime = -1f;

    // Boost values
    bool isSpeedBoosting;
    bool canSpeedBoost;
    bool canReload;

    // Reset variables
    [HideInInspector]
    Vector3 resetPosition;
    float timeOnReset = 0.0f;


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

        resetPosition = transform.position;
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
                StartCoroutine(ChangeFOV(FOV, normalFOV, 0.35f));
            }
        }

        if (bullets <= 0 && canReload)
        {
            canReload = false;
            StartCoroutine(ReloadGun());
        }

        EmitParticles();
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

    void EmitParticles()
    {
        if (Mathf.Abs(currMoveVec.magnitude) > 5f && rb.velocity.y == 0)
            CreateDustParticles();
        else
            StopDustParticles();
    }

    void CreateDustParticles()
    {
        dustParticles.Play();
    }
    void StopDustParticles()
    {
        dustParticles.Stop();
    }

    public void ResetPlayer()
    {
        // Reset position
        rb.velocity = Vector3.zero;
        transform.position = resetPosition;

        // Reset time
        GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>().SetTime(timeOnReset);
    }
}
