using UnityEngine;

public class JumpOrb : Item
{
    public override void GetItem(Player player)
    {
        player.GetJumpOrb();
    }
}
