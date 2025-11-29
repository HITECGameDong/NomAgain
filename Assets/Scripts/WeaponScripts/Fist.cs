using UnityEngine;

public class Fist : Weapon
{
    [SerializeField] float healthAddition = 5f;
    [SerializeField] int expToLevelup = 5;
    int curExp = 0;

    public override void Attack()
    {
        if(currentTargetQueue.Count <= 0) return;

        currentTargetQueue.Dequeue().gameObject.SetActive(false);
        weaponUser.GetHealth(healthAddition);

        curExp++;
        if(curExp >= expToLevelup)
        {
            expToLevelup += 2;
            curExp = 0;
            WeaponLevelUp();
        }
        // 25-11-29 Jin : 현재 체력 보충 이외 사용하지 않음, 필요시 다시 추가하기.
        //weaponUser.onObstacleBroken.Invoke();
    }

    public override void WeaponInit(Player player)
    {
        SetWeaponUser(player);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacles"))
        {
            currentTargetQueue.Enqueue(other.gameObject); 
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacles"))
        {
            currentTargetQueue.Dequeue();
        }
    }

    public override void WeaponLevelUp()
    {
        Debug.Log("FIST LEVEL UP");
        weaponLevel++;
        healthAddition *= 1.2f;
    }
    
    public bool IsFistAttackable()
    {
        return currentTargetQueue.Count > 0;
    }
}
