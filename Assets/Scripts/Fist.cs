using UnityEngine;

public class Fist : Weapon
{
    public override void Attack()
    {
        if(!currentTarget) return;
        currentTarget.SetActive(false);
        onObstacleBroken.Invoke();
    }
}
