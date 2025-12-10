using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SOs/WeaponSO")]
// 25-11-28 TODO-jin : Weapon Type 추가, 단발성? x초 지속후 자기파괴? 등
public class WeaponSO : ItemSO
{
    // 실 장착 Weapon 프리팹임. ItemSO에 있는 itemPrefabList는 땅에 먹을수 있게 스폰되는 프리팹임!
    public GameObject weaponPrefab;
    public Sprite weaponUISprite;

    public override void GetItem(Player player)
    {
        player.GetWeapon(this);
    }
}
