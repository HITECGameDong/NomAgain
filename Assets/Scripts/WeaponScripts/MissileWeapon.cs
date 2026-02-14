using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// 25-11-29 TODO-jin : 이제 쿨 돌면 다시쏘게.
public class MissileWeapon : Weapon
{
    [SerializeField] int breakAmount = 5;
    
    void Update()
    {
        cooltimeTimer += Time.unscaledDeltaTime;
        if(cooltimeTimer >= cooltime)
        {
            cooltimeTimer = 0f;
            Attack();
        }   
    }    

    // 25-12-12 TODO-jin: Async func으로 바꾸기(while간 스레드 넘겨주기)
    public override void Attack()
    {
        cooltimeTimer = 0f;

        Collider[] everyColliders = Physics.OverlapBox(
        range.transform.TransformPoint(range.center), 
        range.bounds.extents,
        Quaternion.identity,
        targetLayer);

        // 미사일 발사
        int index = 0;
        breakAmount = 5;
        while(breakAmount > 0 && everyColliders.Length > index)
        {
            everyColliders[index].gameObject.GetComponent<Obstacle>().DestroyObstacle(this);
            
            index++;
            breakAmount--;
            weaponUser.OnObstacleBroken();
        }
    }

    // 미사일은 습득하자마자 쏩니다.
    public override void WeaponInit(Player player)
    {
        SetWeaponUser(player);
        Attack();
    }

    // 미사일은 습득하자마자 쏩니다.
    public override void WeaponLevelUp()
    {
        Debug.Log("MISSILE LEVEL UP");
        Attack();
        weaponLevel++;
        breakAmount += 2;
    }
}
