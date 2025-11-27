using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    // EVENTS
    public UnityEvent onTilePassing;
    public UnityEvent onArrivingCheckpoint;
    public UnityEvent onPlayerDead;
    public UnityEvent<float> onItemGet;
    // VARIABLES FROM EDITOR / COMPONENTS
    PlayerMovement playerMovement;
    [SerializeField] GameManager gameManager;
    // TODO : this is terrible idea to set it as public. did it for scoremanager , also remove SerializeField
    [SerializeField] public Weapon equippedWeapon;
    
    // CONSTANTS..
    [SerializeField] float initLocX = -14f;
    [SerializeField] float fatigueRate = 2f;
    [SerializeField] public float maxHealth {get; private set;} = 100f; 

    // VARIABLES
    //GameObject currentSteppingBlock = null;
    bool isBlockBreakable = false;
    bool isVulnerable = true;
    public float health {get; private set;}
    bool isItemWorking = false;
    float gainedItemDuration = 0f;
    IItem grabbableItem = null;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        health = maxHealth;
    }

    void Start()
    {
        playerMovement.onItemWorkingDone.AddListener(ItemWorkingDone);
        equippedWeapon.onObstacleBroken.AddListener(OnBreakingBlock);
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
            // TODO : check obstacle type and get diffrent damages
            GetDamage(30f);
        }

        if (other.gameObject.CompareTag("HitLine"))
        {
            isBlockBreakable = true;
        }

        if(other.gameObject.CompareTag("PassLine"))
        {
            onTilePassing.Invoke();
        }

        if(other.gameObject.CompareTag("Item"))
        {
            grabbableItem = other.GetComponent<IItem>();
        }

        // Get Block Obj that player stands.
        // if(other.gameObject.CompareTag("BlockSelector"))
        // {
        //     currentSteppingBlock = other.gameObject;
        // }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HitLine"))
        {
            isBlockBreakable = false;
        }

        if (other.gameObject.CompareTag("Item"))
        {
            grabbableItem = null;
        }
    } 

    void InputChecking()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(isBlockBreakable)
            {
                HitBlock();
            }
            else if(grabbableItem != null)
            {
                ItemGrabCheck();
            }
            else Jump();
        }
        
        else if (Mouse.current.leftButton.isPressed)
        {
            Jump();
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
    }

    public void GetEnergyBoost(float speedAddition, float duration, float healthAddition)
    {
        isItemWorking = true;
        GetHealth(healthAddition);
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

    void ItemGrabCheck()
    {
        if(!isItemWorking)
        {
            if(grabbableItem == null) return;
            
            grabbableItem.GetItem(this);
            onItemGet.Invoke(gainedItemDuration);
            gainedItemDuration = 0f;
            grabbableItem = null;
        }
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

    // public GameObject GetCurrentSteppingBlock()
    // {
    //     // WARN : 얕은복사. 원본 변경 주의
    //     return currentSteppingBlock;
    // }

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

    void GetHealth(float amount)
    {
        health = Mathf.Min(amount + health, maxHealth);
    }

    void GetDamage(float amount)
    {
        health = Mathf.Max(health - amount, 0f);
    }

    void HitBlock()
    {
        equippedWeapon.Attack();
        isBlockBreakable = false;
    }

    void OnBreakingBlock()
    {
        GetHealth(7f);
        isBlockBreakable = false;
    }

    public float GetCurrentSpeed()
    {
        return playerMovement.GetCurrentSpeed();
    }

    public float GetBaseSpeed()
    {
        return playerMovement.GetBaseSpeed();
    }

    public void IncreaseDefaultSpeed(float addition)
    {
        playerMovement.IncreaseSpeed(addition);
    }
}
