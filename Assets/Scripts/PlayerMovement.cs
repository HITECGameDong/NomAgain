using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float moveSpeed = 4f;
    
    void Awake()
    {
        playerTransform = GetComponent<Transform>();
    }

  void Update()
    {
        playerTransform.Translate(Vector3.right * moveSpeed * Time.deltaTime);    
    }
}
