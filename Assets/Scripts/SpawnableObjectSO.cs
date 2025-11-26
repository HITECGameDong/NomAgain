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

public enum ObjectName
{
    NONE,
    GROUND,
    SINGLE_BLOCK,
    ENERGY_ITEM,
    ROCKET_ITEM,
}

[CreateAssetMenu(fileName = "ObjectSO", menuName = "SOs/ObjectSO")]
public class SpawnableObjectSO : ScriptableObject
{
    public GameObject itemPrefab;
    public ObjectType objectType;
    public ObjectName objectName;
    public float spawnWeight;
}
