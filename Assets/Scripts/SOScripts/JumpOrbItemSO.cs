using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSO", menuName = "SOs/Item/JumpOrb")]
public class JumpOrbItemSO : ItemSO
{
    public override void GetItem(Player player)
    {
        player.GetJumpOrb();
    }
}
