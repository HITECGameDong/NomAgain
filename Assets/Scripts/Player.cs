using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerMovement playerMovement;
    BoxCollider boxCollider;
    Rigidbody rb;

    bool isActionable = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Button Pressed");
            if(isActionable)
            {
                Jump();
            }
            else
            {
                Debug.Log("Aciton Failed");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("Game Over");
            playerMovement.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HitLine"))
        {
            isActionable = true;
            Debug.Log("Action Available (Trigger)");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HitLine"))
        {
            isActionable = false;
            Debug.Log("Action Unavailable (Trigger)");
        }
    } 


    void Jump()
    {
        rb.AddForce(Vector3.up * 7f, ForceMode.Impulse);
        isActionable = false;
    }
}
