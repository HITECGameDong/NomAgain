using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// 25-11-29 TODO-jin : 이제 쿨 돌면 다시쏘게.
public class MissileWeapon : Weapon
{
    [SerializeField] int breakAmount = 5;

    // 미사일은 습득하자마자 쏩니다.
    public override void Attack()
    {
        return;
    }

    // 미사일은 습득하자마자 쏩니다.
    public override void WeaponInit(Player player)
    {
        SetWeaponUser(player);

        Collider[] everyColliders = Physics.OverlapBox(
            range.transform.TransformPoint(range.center), 
            range.bounds.extents,
            Quaternion.identity,
            targetLayer);
        
        if(everyColliders.Length == 0)
        {
            // weaponUser.UnequipWeapon();
            return;
        }

        // 미사일 발사
        int index = 0;
        while(breakAmount > 0 && everyColliders.Length > index)
        {
            everyColliders[index].transform.gameObject.SetActive(false);
            
            index++;
            breakAmount--;
            weaponUser.onObstacleBroken.Invoke();
        }

        // 다쐈으면 무기파괴
        // weaponUser.UnequipWeapon();
    }

    public override void WeaponLevelUp()
    {
        Debug.Log("MISSILE LEVEL UP");
        weaponLevel++;
    }
}
