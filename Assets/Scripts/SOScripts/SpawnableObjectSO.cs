using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObjectType
{
    NONE,
    GROUND,
    ITEM,
    OBSTACLE,
}

public class SpawnableObjectSO : ScriptableObject
{
    // 테마별 다른 스폰 프리팹 리스트. 바닥에 스폰되는거임! 장착하는 무기 프리팹은 WeaponSO.cs에 정리!
    public List<GameObject> itemPrefabList;
    public ObjectType objectType;

    // 스폰 확률, 100이라고 100퍼 이거만 생성하는건 아님. 누적합 방식 랜덤 뽑기 채용(ObjectSpawner.cs 참조) 
    // 25-11-27 WARN-jin : 현재 Ground의 경우엔 weight 무시하고 플레이어 위치따라 스폰중임.
    [Range(0f, 100f)]
    public float spawnWeight;
}
