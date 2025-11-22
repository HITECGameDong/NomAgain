using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float moveSpeed = 4f;
    float speedAddition = 0f;
    Rigidbody rb;
    

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        playerTransform.Translate(Vector3.right * (moveSpeed + speedAddition) * Time.deltaTime);    
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * 7f, ForceMode.Impulse);
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
    }

    public float GetCurrentSpeed()
    {
        return moveSpeed + speedAddition;
    }

    public void RocketBoost(float rocketSpeed, float duration)
    {
        StartCoroutine(RocketBoostCoroutine(rocketSpeed, duration));
    }

    System.Collections.IEnumerator RocketBoostCoroutine(float rocketSpeed, float duration)
    {
        float lastMoveSpeed = moveSpeed;

        moveSpeed = rocketSpeed;
        rb.useGravity = false;
        playerTransform.position = new Vector3(playerTransform.position.x, 10f, playerTransform.position.z);
        yield return new WaitForSeconds(duration);
        moveSpeed = lastMoveSpeed;
        rb.useGravity = true;
    }
}
