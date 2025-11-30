using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    // EVENTS
    public UnityEvent onItemWorkingDone;
    // VARIABLES FROM EDITOR / COMPONENTS
    Transform playerTransform;
    Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundChecker;
    
    // CONSTANTS
    [SerializeField] int maxJumpCount = 1;
    [SerializeField] float jumpForce = 18f;
    [SerializeField] float gravityMult = 4.5f;
    [SerializeField] float halfGravThresSpeed= -16f;

    // VARIABLES
    [SerializeField] float baseSpeed = 4f;
    float speedAddition = 0f;
    float halfGravOrFullGrav = 1f;
    int jumpCount;
    bool isFlying = true;
    bool isOnRocket = false;

    

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        jumpCount = maxJumpCount;
    }

    void Update()
    {
        playerTransform.Translate(Vector3.right * (baseSpeed + speedAddition) * Time.deltaTime);    
    }

    void FixedUpdate()
    {
        OnGroundAction();
        GravityWorking();
    }

    void OnGroundAction()
    {  
        bool isOnGround = Physics.CheckSphere(groundChecker.position, 0.1f, groundLayer);
        if(isOnGround)
        {
            if(!isFlying)
            {
                RefillJump();
            } 
            isFlying = false;
        }
    }

    void GravityWorking()
    {
        if(isOnRocket) return;
        halfGravOrFullGrav = (rb.linearVelocity.y >= halfGravThresSpeed) ? 0.5f : 1f; 
        rb.linearVelocity += Vector3.up * Physics.gravity.y * gravityMult * halfGravOrFullGrav * Time.fixedDeltaTime;
    }

    void RefillJump()
    {
        jumpCount = maxJumpCount;
    }

    public void Jump()
    {
        if(jumpCount <= 0) return;
        if(isOnRocket) return;

        isFlying = true;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        jumpCount--;
    }

    public void IncreaseSpeed(float addition)
    {
        speedAddition += addition;
    }

    public void IncreaseSpeed(float addition, float duration)
    {
        StartCoroutine(IncreaseSpeedCoroutine(addition, duration));
    }

    System.Collections.IEnumerator IncreaseSpeedCoroutine(float addition, float duration)
    {
        speedAddition += addition;
        yield return new WaitForSeconds(duration);
        speedAddition -= addition;

        onItemWorkingDone.Invoke();
    }

    public float GetCurrentSpeed()
    {
        return baseSpeed + speedAddition;
    }

    public float GetBaseSpeed()
    {
        return baseSpeed;
    }

    public void RocketBoost(float rocketSpeed, float duration)
    {
        StartCoroutine(RocketBoostCoroutine(rocketSpeed, duration));
    }

    System.Collections.IEnumerator RocketBoostCoroutine(float rocketSpeed, float duration)
    {
        float lastMoveSpeed = speedAddition;

        speedAddition = rocketSpeed;
        rb.useGravity = false;
        isFlying = true;
        isOnRocket = true;
        playerTransform.position = new Vector3(playerTransform.position.x, 10f, playerTransform.position.z);
        yield return new WaitForSeconds(duration);
        speedAddition = lastMoveSpeed;
        rb.useGravity = true;
        isOnRocket = false;

        onItemWorkingDone.Invoke();
    }
}
