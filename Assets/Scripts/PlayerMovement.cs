using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float moveSpeed = 4f;
    float speedAddition = 0f;
    
    void Awake()
    {
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        playerTransform.Translate(Vector3.right * (moveSpeed + speedAddition) * Time.deltaTime);    
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
}
