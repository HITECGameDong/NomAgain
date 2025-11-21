using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject[] objs;
    [SerializeField] Transform nextLocation;

    [SerializeField] float offsetX = 34f;

    void Start()
    {
        player.onTilePassing.AddListener(SpawnObject);
    }

    void SpawnObject()
    {
        int randomIndex = Random.Range(0, objs.Length);
        Instantiate(objs[randomIndex], nextLocation.position, Quaternion.identity);
        nextLocation.position += new Vector3(offsetX, 0f, 0f);
    }
}
