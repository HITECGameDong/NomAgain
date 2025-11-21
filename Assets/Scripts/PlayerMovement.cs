using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float moveSpeed = 4f;
    float speedAddition = 0f;
    Rigidbody rb;
    
    void Awake()
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
        Debug.Log("BOOST START");
        yield return new WaitForSeconds(duration);
        speedAddition -= addition;
        Debug.Log("BOOST DONE");
    }

    public float GetCurrentSpeed()
    {
        return moveSpeed + speedAddition;
    }
}
