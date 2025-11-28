using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SOs/WeaponSO")]
// 25-11-28 TODO-jin : Weapon Type 추가, 단발성? x초 지속후 자기파괴? 등
public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
}
