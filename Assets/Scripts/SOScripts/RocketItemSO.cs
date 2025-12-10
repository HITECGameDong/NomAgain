using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSO", menuName = "SOs/Item/Rocket")]
public class RocketItemSO : ItemSO
{
    [SerializeField] float speedAddition = 30f;
    [SerializeField] float duration = 2f;

    // 25-11-27 TODO-jin : addition / duration err catch넣기
    public override void GetItem(Player player)
    {
       player.GetRocketBoost(speedAddition, duration, itemName);
    }
}
