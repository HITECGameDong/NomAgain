using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{

    public UnityEvent onTilePassing;
    PlayerMovement playerMovement;
    
    public UnityEvent onArrivingCheckpoint;

    [SerializeField] float initLocX = -14f;
    GameObject currentSteppingBlock;

    [SerializeField] GameManager gameManager;

    bool isActionable = false;

    public float health {get; private set;}
    [SerializeField] public float maxHealth {get; private set;} = 100f; 

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentSteppingBlock = null;

        health = maxHealth;
    }


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(isActionable)
            {
                Jump();
            }
        }
        
        if(gameObject.transform.position.x >= gameManager.GetResetLoc())
        {
            onArrivingCheckpoint.Invoke();
        }

        HealthDoSomething();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            GetDamage(30f);
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

        if(other.gameObject.CompareTag("BlockSelector"))
        {
            currentSteppingBlock = other.gameObject;
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

    public void GetEnergyBoost(float speedAddition, float duration, float healthAddition)
    {
        AddHealth(healthAddition);
        playerMovement.IncreaseSpeed(speedAddition, duration);
    }

    public void ResetPosition()
    {
        Vector3 resetPosition = new Vector3(gameObject.transform.position.x - gameManager.GetResetLoc(), 
            gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.position = resetPosition;
    }

    public GameObject GetCurrentSteppingBlock()
    {
        // WARN : 얕은복사. 원본 변경 주의
        return currentSteppingBlock;
    }

    public float GetCurrentLocation()
    {
        return gameObject.transform.position.x;
    }

    public float GetInitLocation()
    {
        return initLocX;
    }

    void HealthDoSomething()
    {
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("DEADDEAD");
        playerMovement.enabled = false;
    }

    void AddHealth(float amount)
    {
        health = Mathf.Min(amount + health, maxHealth);
    }

    void GetDamage(float amount)
    {
        health = Mathf.Max(health - amount, 0f);
    }
}
