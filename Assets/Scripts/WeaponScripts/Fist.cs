using UnityEngine;

// 25-11-29 WARN-jin : 현재 장애물 제거 안됨!!!!!!!!!!!!! 수정예정 하단 TODO 참조
public class Fist : Weapon
{
    public override void Attack()
    {
        // 25-11-27 jin : Player에서 이제 Fist 사용 여부 체크할 필요가 없음. 
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
        // 25-11-29 TODO-jin : HitLine->Obstacle 태그, Range안에 Obstacle이 있으면 되게. 그리고 Range 확줄이기.
        if(other.gameObject.CompareTag("HitLine"))
        {
            currentTargetQueue.Enqueue(other.gameObject); 
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("HitLine"))
        {
            currentTargetQueue.Dequeue();
        }
    }

    public override void WeaponLevelUp()
    {
        Debug.Log("FIST LEVEL UP");
        weaponLevel++;
    }
}
