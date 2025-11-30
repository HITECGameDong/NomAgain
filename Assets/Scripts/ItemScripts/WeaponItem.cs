using UnityEngine;

public class WeaponItem : Item
{
    // jin- 이거 public이 맞는건가 
    [SerializeField] WeaponSO weaponSO;

    public override void GetItem(Player player)
    {
        player.GetWeapon(weaponSO);
        gameObject.SetActive(false);
    }
}
