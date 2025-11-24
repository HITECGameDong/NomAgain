using UnityEngine;

public class EnergyBooster : Item
{
    [SerializeField] float speedAddition = 2f;
    [SerializeField] float duration = 2f;
    [SerializeField] float healthAddition = 30f;

    void Awake()
    {
        itemType = ItemType.ENERGY;
    }

    public override void GetItem(Player player)
    {
       player.GetEnergyBoost(speedAddition, duration, healthAddition);
       gameObject.SetActive(false);
    }
}
