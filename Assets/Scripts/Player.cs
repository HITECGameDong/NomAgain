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
    bool isVulnerable = false;
    public UnityEvent onPlayerDead;
    [SerializeField] public float maxHealth {get; private set;} = 100f; 
    [SerializeField] float fatigueRate = 2f;

    public UnityEvent<float> onItemGet;
    bool isItemWorking = false;
    float gainedItemDuration = 0f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentSteppingBlock = null;

        health = maxHealth;
    }

    void Start()
    {
        playerMovement.onItemWorkingDone.AddListener(ItemWorkingDone);
    }


    void Update()
    {
        InputChecking();
        CheckToResetPos();
        HealthDoSomething();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacles"))
        {
            GetDamage(30f);
        }

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

            if(!isItemWorking)
            {
                item.GetItem(this);
            }  
            
            // TODO : no destroy. item pooling
            Destroy(other.gameObject);
            onItemGet.Invoke(gainedItemDuration);
            gainedItemDuration = 0f;
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

    void InputChecking()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(isActionable)
            {
                Jump();
            }
        }
    }

    void CheckToResetPos()
    {
        if(gameObject.transform.position.x >= gameManager.GetResetLoc())
        {
            onArrivingCheckpoint.Invoke();
        }    
    }

    void Jump()
    {
        playerMovement.Jump();
        isActionable = false;
    }

    public void GetEnergyBoost(float speedAddition, float duration, float healthAddition)
    {
        isItemWorking = true;
        AddHealth(healthAddition);
        playerMovement.IncreaseSpeed(speedAddition, duration);
        gainedItemDuration = duration;
    }

    public void GetRocketBoost(float speedAddition, float duration)
    {
        isItemWorking = true;
        StartCoroutine(GetRocketBoostCoroutine(duration));
        playerMovement.RocketBoost(speedAddition, duration);
        gainedItemDuration = duration;
    }

    void ItemWorkingDone()
    {
        isItemWorking = false;
    }

    System.Collections.IEnumerator GetRocketBoostCoroutine(float duration)
    {
        isVulnerable = false;
        yield return new WaitForSeconds(duration);
        isVulnerable = true;
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
        if(!isVulnerable) return;

        GetDamage(fatigueRate * Time.deltaTime);
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        onPlayerDead.Invoke();
    }

    public void Kill()
    {
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
