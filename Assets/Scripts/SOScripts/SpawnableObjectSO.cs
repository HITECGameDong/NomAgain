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

[CreateAssetMenu(fileName = "ObjectSO", menuName = "SOs/ObjectSO")]
public class SpawnableObjectSO : ScriptableObject
{
    // WARN-jin : 난이도 테마별 프리팹 저장, 난이도 테마 순서대로 저장할것! ex) 0: 이지, 1: 하드, 2: 지옥 테마 프리팹
    public List<GameObject> itemPrefabList;
    public ObjectType objectType;

    // 스폰 확률, 100이라고 100퍼 이거만 생성하는건 아님. 누적합 방식 랜덤 뽑기 채용(ObjectSpawner.cs 참조) 
    // 25-11-27 WARN-jin : 현재 Ground의 경우엔 weight 무시하고 플레이어 위치따라 스폰중임.
    [Range(0f, 100f)]
    public float spawnWeight;
}
