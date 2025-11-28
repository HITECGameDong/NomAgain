using UnityEngine;

public class WeaponItem : MonoBehaviour, IItem
{
    // jin- 이거 public이 맞는건가 
    [SerializeField] WeaponSO weaponSO;

    public void GetItem(Player player)
    {
        player.GetWeapon(weaponSO);
        gameObject.SetActive(false);
    }
}
