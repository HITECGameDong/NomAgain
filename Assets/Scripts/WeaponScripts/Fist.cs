using UnityEngine;

public class Fist : Weapon
{
    public override void Attack()
    {
        if(currentTargetQueue.Count <= 0) return;

        currentTargetQueue.Dequeue().gameObject.SetActive(false);
        weaponUser.onObstacleBroken.Invoke();
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
    }
    
    public bool IsFistAttackable()
    {
        return currentTargetQueue.Count > 0;
    }
}
