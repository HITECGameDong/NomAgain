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
    // VARIABLES
    [SerializeField] float baseSpeed = 4f;
    float speedAddition = 0f;
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

    void RefillJump()
    {
        jumpCount = maxJumpCount;
    }

    public void Jump()
    {
        if(jumpCount <= 0) return;
        if(isOnRocket) return;

        isFlying = true;
        rb.AddForce(Vector3.up * 7f, ForceMode.Impulse);
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

    public void RocketBoost(float rocketSpeed, float duration)
    {
        StartCoroutine(RocketBoostCoroutine(rocketSpeed, duration));
    }

    System.Collections.IEnumerator RocketBoostCoroutine(float rocketSpeed, float duration)
    {
        float lastMoveSpeed = baseSpeed;

        baseSpeed = rocketSpeed;
        rb.useGravity = false;
        isFlying = true;
        isOnRocket = true;
        playerTransform.position = new Vector3(playerTransform.position.x, 10f, playerTransform.position.z);
        yield return new WaitForSeconds(duration);
        baseSpeed = lastMoveSpeed;
        rb.useGravity = true;
        isOnRocket = false;

        onItemWorkingDone.Invoke();
    }
}
