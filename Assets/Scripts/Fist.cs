using UnityEngine;

public class Fist : Weapon
{
    public override void Attack()
    {
        // 25-11-27 jin : Player에서 이제 Fist 사용 여부 체크할 필요가 없음. 
        if(currentTargetQueue.Count <= 0) return;

        currentTargetQueue.Dequeue().gameObject.SetActive(false);
        onObstacleBroken.Invoke();
    }

    public override void WeaponInit(Player player)
    {
        SetWeaponUser(player);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("HitLine"))
        {
            currentTargetQueue.Enqueue(other.transform.root.gameObject); 
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("HitLine"))
        {
            currentTargetQueue.Dequeue();
        }
    }
}
