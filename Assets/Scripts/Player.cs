using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public UnityEvent onTilePassing;
    PlayerMovement playerMovement;
    
    public UnityEvent onArrivingCheckpoint;

    [SerializeField] float initLocX = -14f;
    readonly float checkPointX = 50f;
    GameObject currentSteppingBlock;


    bool isActionable = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentSteppingBlock = null;
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
        
        if(gameObject.transform.position.x >= checkPointX)
        {
            onArrivingCheckpoint.Invoke();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
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

    public void GetEnergyBoost(float speedAddition, float duration)
    {
        playerMovement.IncreaseSpeed(speedAddition, duration);
    }

    public void ResetPosition()
    {
        Vector3 resetPosition = new Vector3(initLocX, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.position = resetPosition;
    }

    public GameObject GetCurrentSteppingBlock()
    {
        // WARN : 얕은복사. 원본 변경 주의
        return currentSteppingBlock;
    }
}
