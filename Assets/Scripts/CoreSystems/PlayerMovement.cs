using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

// RUN : 최초, Ground 인식시.
// JUMP : Jump 시작시
// ON_ROCKET : Rocket 시작시
public enum PlayerState
{
    RUN,
    JUMP,
    ON_ROCKET,
}

public class PlayerMovement : MonoBehaviour
{
    // EVENTS
    public UnityEvent onItemWorkingDone;
    // VARIABLES FROM EDITOR / COMPONENTS
    [SerializeField] Player player;
    Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundChecker;
    
    // CONSTANTS
    [SerializeField] int maxJumpCount = 1;
    [SerializeField] float jumpForce = 18f;
    [SerializeField] float gravityMult = 4.5f;
    [SerializeField] float halfGravThresSpeed= -16f;

    // VARIABLES
    Transform playerTransform;
    [SerializeField] float baseSpeed = 4f;
    float speedAddition = 0f;
    float halfGravOrFullGrav = 1f;
    int jumpCount;
    bool isFlying = true;
    bool isGravityOn = true;
    // jin : Player에서 RocketStomp목적 사용
    public PlayerState state = PlayerState.RUN;

    
    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
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
            // 착지 직후 인식
            if(!isFlying)
            {
                // Rocket 이후 착지하면, Stomp!
                if(state == PlayerState.ON_ROCKET)
                {
                    player.RocketStomp();
                }
                RefillJump();
                state = PlayerState.RUN;

            } 
            isFlying = false;
        }
    }

    // jin: 기본 Rigidbody에 작용하는 Gravity의 addition임. rb.useGravity 설정과 따로 놀음. Movement 내부 함수 EnableGravity() 등을 사용하기.
    void GravityWorking()
    {
        if(!isGravityOn) return;

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
        if(state != PlayerState.RUN && state != PlayerState.JUMP) return;

        state = PlayerState.JUMP;
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
        float lastMoveSpeedAdd = speedAddition;

        // Rocket State는 착지시 원복예정.
        state = PlayerState.ON_ROCKET;
        speedAddition = rocketSpeed;
        DisableGravity();
        // Ground Check 두번 방지 관련 설정
        isFlying = true;
        playerTransform.position = new Vector3(playerTransform.position.x, 10f, playerTransform.position.z);

        yield return new WaitForSeconds(duration);

        speedAddition = lastMoveSpeedAdd;
        EnableGravity();

        onItemWorkingDone.Invoke();
    }

    void DisableGravity()
    {
        rb.useGravity = false;
        isGravityOn = false;
    }

    void EnableGravity()
    {
        rb.useGravity = true;
        isGravityOn = true;
    }
}
