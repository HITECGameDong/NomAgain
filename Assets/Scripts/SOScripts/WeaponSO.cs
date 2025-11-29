using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SOs/WeaponSO")]
// 25-11-28 TODO-jin : Weapon Type 추가, 단발성? x초 지속후 자기파괴? 등
// 25-11-29 TODO-jin : 굳이 이거 써야함? 온리 스폰용 SO인데.. 기왕 넣을거면 쿨타임 조절도 되도록.
public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
}
