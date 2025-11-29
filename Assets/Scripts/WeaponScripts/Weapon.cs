using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Player가 직접 SO를 통한 EquipWeapon() , UnequipWeapon() 함수 호출로 프리팹을 생성해서 장착. 
// 25-11-28 WARN-jin : 절대로 Weapon이 자기를 Destroy()하게 하지 말것! 그럼 Player는 Unequip시 SO 내부 Prefab을 지우려 애씀(Editor에서 막긴함)
public abstract class Weapon : MonoBehaviour
{
    protected BoxCollider range;
    protected Queue<GameObject> currentTargetQueue = new Queue<GameObject>();
    [SerializeField] protected LayerMask targetLayer;
    protected Player weaponUser;
    public int weaponLevel {get; protected set;} = 1;
    [SerializeField] public float cooltime;    
    public float cooltimeTimer = 0f;
    public WeaponSO weaponSO;

    void Awake()
    {
        if(!gameObject.TryGetComponent<BoxCollider>(out range))
        {
            Debug.LogWarning("Weapon Prefab에 BoxCollider 컴포넌트를 달으세요");
            return;
        }

        // 각 weapon별 targetLayer 미설정시
        if(targetLayer == 0)
        {
            Debug.LogWarning("Weapon TargetLayer 미설정, Weapon prefab 확인하세요");
            return;
        }
    }

    // WARN-jin : 꼭 오브젝트 부술때 player.onObstacleBroken Invoke할것.
    public abstract void Attack();

    // WARN-jin : 꼭 무기 습득시 Player가 호출할 것
    public virtual void WeaponInit(Player player)
    {
        SetWeaponUser(player);
    }

    protected virtual void SetWeaponUser(Player player)
    {
        weaponUser = player;
    }

    public abstract void WeaponLevelUp();

    public bool IsAttackable()
    {
        return currentTargetQueue.Count > 0;
    }
}
