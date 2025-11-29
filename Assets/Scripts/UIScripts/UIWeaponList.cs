using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class UIWeaponList : MonoBehaviour
{
    [SerializeField] List<UIWeaponTile> tileList = new List<UIWeaponTile>();

    public void UpdateWeaponUI(Weapon weapon)
    {
        foreach(UIWeaponTile tile in tileList)
        {
            if(tile.isEmpty)
            {
                tile.AddWeapon(weapon);
                return;
            }

            if(tile.weapon == weapon)
            {
                //tile.UpdateWeaponUI(weapon);
            }
        }
    }

    public void HideUI()
    {
        foreach(UIWeaponTile tile in tileList)
        {
            tile.HideUI();
        }
    }
}
