using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public UnityEvent onTilePassing;
    PlayerMovement playerMovement;
    BoxCollider boxCollider;


    bool isActionable = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        playerMovement = GetComponent<PlayerMovement>();
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
        }

        if(other.gameObject.CompareTag("PassLine"))
        {
            onTilePassing.Invoke();
        }

        if(other.gameObject.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            item.GetItem(this);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HitLine"))
        {
            isActionable = false;
        }
    } 


    void Jump()
    {
        playerMovement.Jump();
        isActionable = false;
    }

    public void GetEnergyBoost(float speedAddition, float duration)
    {
        playerMovement.IncreaseSpeed(speedAddition, duration);
    }
}
