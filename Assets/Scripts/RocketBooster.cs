using UnityEngine;

public class RocketBooster : Item
{
    [SerializeField] float speedAddition = 30f;
    [SerializeField] float duration = 2f;

    void Awake()
    {
        itemType = ItemType.ROCKET;
    }

    public override void GetItem(Player player)
    {
       player.GetRocketBoost(speedAddition, duration);
    }
}
