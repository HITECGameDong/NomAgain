using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    NONE,
    ENERGY,
    ROCKET,
}

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
    public GameObject itemPrefab;
    public ObjectType objectType;
    public float spawnWeight;
}
