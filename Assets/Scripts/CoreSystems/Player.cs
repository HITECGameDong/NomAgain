using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    // EVENTS
    public UnityEvent onTilePassing;
    public UnityEvent onArrivingCheckpoint;
    public UnityEvent onPlayerDead;
    public UnityEvent<float> onItemGet;
    public UnityEvent onObstacleBroken;
    public UnityEvent<Weapon> onWeaponGet;

    // VARIABLES FROM EDITOR / COMPONENTS
    PlayerMovement playerMovement;
    [SerializeField] GameManager gameManager;
    [SerializeField] WeaponSO baseWeaponSO;
    [SerializeField] BoxCollider rocketStompRange;
    [SerializeField] LayerMask obstacleLayer;

    // CONSTANTS..
    [SerializeField] float initLocX = -14f;
    [SerializeField] float fatigueRate = 2f;
    [SerializeField] public float maxHealth {get; private set;} = 100f; 

    // VARIABLES
    //GameObject currentSteppingBlock = null;
    Dictionary<Type, Weapon> equippedWeapons = new Dictionary<Type, Weapon>();
    GameObject equippedWeaponGO;
    bool isVulnerable = true;
    public float health {get; private set;}
    bool isItemWorking = false;
    float gainedItemDuration = 0f;
    Item grabbableItem = null;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
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
            if(playerMovement.state == PlayerState.ON_ROCKET)
            {
                isVulnerable = false;
            }
            // TODO : check obstacle type and get diffrent damages
            GetDamage(30f);
        }

        if(other.gameObject.CompareTag("PassLine"))
        {
            onTilePassing.Invoke();
        }

        if(other.gameObject.CompareTag("Item"))
        {
            grabbableItem = other.GetComponent<Item>();
        }

        // Get Block Obj that player stands.
        // if(other.gameObject.CompareTag("BlockSelector"))
        // {
        //     currentSteppingBlock = other.gameObject;
        // }
    }

    void RocketStomp(GameObject objToDestroy)
    {
        if(rocketStompRange == null)
        {
            Debug.LogWarning("Player 오브젝트 내에 RocketStompRange 객체가 없음");
            return;
        }

        objToDestroy.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            grabbableItem = null;
        }
    } 

    void InputChecking()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(equippedWeapons[typeof(Fist)].IsAttackable())
            {
                PunchBlock();
            }
            // 25-11-29 TODO-jin : 이러면 무기도 못먹음. energy 효과적용간에는 먹을수있어야함.
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

    public void GetHealth(float amount)
    {
        if(!isVulnerable && amount < 0)
        {
            return;
        }
        health = Mathf.Min(amount + health, maxHealth);
    }

    void GetDamage(float amount)
    {
        if(!isVulnerable) return;
        health = Mathf.Max(health - amount, 0f);
    }

    void PunchBlock()
    {
        equippedWeapons[typeof(Fist)].Attack();
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

    public void GetWeapon(WeaponSO weaponToEquipSO)
    {
        if(weaponToEquipSO == null)
        {
            Debug.LogWarning("Player가 받은 Item SO가 null");
            return;
        }

        EquipWeapon(weaponToEquipSO);
    }


    //문제 : 미사일 먹고 미사일무기 지가 지움, eplayer equipp에는 SO 내부 프리팹의 weapon 컴포넌트가 들어가있음.
    // 그럼 이후에 SO 프리팹을 지우게됨.
    // 무기가 지를 지우지말고, 무기가 player에게 unequip을 요청한다. player는 그걸지우기 equipped를 null로 변경 / fist로 변경
    // 그럼 생성된 player의 프리팹은 어떻게 지우는가?
    void EquipWeapon(WeaponSO weaponToEquipSO)
    {   
        if(weaponToEquipSO.weaponPrefab == null)
        {
            Debug.LogWarning("Player가 습득한 WeaponSO 내부에 생성할 Prefab이 없음");
            return;
        }

        // Weapon Spawn시, Player 내부 Local Position만을 변경해 웨폰 위치를 잡음.
        equippedWeaponGO = Instantiate(weaponToEquipSO.weaponPrefab, this.transform);
        equippedWeaponGO.transform.position = new Vector3(0f,0f,0f);
        equippedWeaponGO.transform.localPosition = new Vector3(weaponToEquipSO.weaponPrefab.transform.localScale.x * 0.5f, 0f, 0f);
        
        if(!equippedWeaponGO.TryGetComponent<Weapon>(out Weapon equippedWeapon))
        {
            Debug.LogWarning("Weapon Prefab에 Weapon 컴포넌트가 없음");
            Destroy(equippedWeaponGO);
            return;   
        }

        // 이미 습득했었던 Weapon이면, Levelup.
        if(!equippedWeapons.TryAdd(equippedWeapon.GetType(), equippedWeapon))
        {
            Debug.Log("기존 등록 Item, Level UP");
            Destroy(equippedWeaponGO);
            equippedWeapons[equippedWeapon.GetType()].WeaponLevelUp();
            return;
        }


        // jin: 꼭 호출하세요! else weapon은 player 정보가 없어 작동불가
        equippedWeapon.WeaponInit(this);
        onWeaponGet.Invoke(equippedWeapon);
    }
    
    public void PlayerInit()
    {
        EquipWeapon(baseWeaponSO);
    }

    // 자동 파괴 무기의 경우 해당 무기가 직접 호출함.
    // public void UnequipWeapon()
    // {
    //     if(equippedWeapon == null)  return;
    //     if(equippedWeaponGO == null) return;

    //     equippedWeapon = null;
    //     Destroy(equippedWeaponGO);
    // }

    // PlayerMovement 코드에서 Rocket상태에서 Ground 착지 확인 후 직접 호출함. 
    public void RocketStomp()
    {
        Collider[] everyObstacles = Physics.OverlapBox(
        rocketStompRange.transform.TransformPoint(rocketStompRange.center), 
        rocketStompRange.bounds.extents,
        Quaternion.identity,
        obstacleLayer);

        foreach(Collider eachCollider in everyObstacles)
        {
            eachCollider.gameObject.SetActive(false);
            onObstacleBroken.Invoke();
        }

        // Rocket 상태 끝나면, 피해 입음
        isVulnerable = true;
    }
}
