using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSO", menuName = "SOs/Item/Energy")]
public class EnergyItemSO : ItemSO
{
    public float speedAddition = 2f;
    public float duration = 2f;
    public float healthAddition = 30f;

    // 25-11-27 TODO-jin : addition / duration err catch넣기
    public override void GetItem(Player player)
    {    
        player.GetEnergyBoost(speedAddition, duration, healthAddition);
    }
}
