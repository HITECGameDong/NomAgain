using UnityEngine;

public class EnergyBooster : Item
{
    [SerializeField] float speedAddition = 2f;
    [SerializeField] float duration = 2f;
    [SerializeField] float healthAddition = 30f;

    // 25-11-27 TODO-jin : addition / duration err catch넣기
    public override void GetItem(Player player)
    {    
        player.GetEnergyBoost(speedAddition, duration, healthAddition);
        gameObject.SetActive(false);
    }
}
