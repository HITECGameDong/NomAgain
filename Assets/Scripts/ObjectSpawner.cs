using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject[] objs;
    [SerializeField] Transform nextLocation;
    [SerializeField] float offsetX = 68f;

    // POOLER
    [SerializeField] Transform obstacleParent;
    Queue<GameObject> pool = new Queue<GameObject>();
    const int poolSize = 4;


    void Start()
    {
        player.onTilePassing.AddListener(SpawnAndPoolingObject);

        //POOLER, get created objects first, REMOVE LATER
        foreach(Transform child in obstacleParent)
        {
            pool.Enqueue(child.gameObject);
        }
    }

    GameObject SpawnObjectAndGet()
    {
        int randomIndex = Random.Range(0, objs.Length);
        GameObject obj = Instantiate(objs[randomIndex], nextLocation.position, Quaternion.identity, obstacleParent);

        nextLocation.position += new Vector3(offsetX, 0f, 0f);
        
        return obj;
    }


    void SpawnAndPoolingObject()
    {
        if(poolSize <= pool.Count)
        {
            GameObject objToDestroy = pool.Dequeue();
            Destroy(objToDestroy);
            pool.Enqueue(SpawnObjectAndGet());
        }

        else
        {
            pool.Enqueue(SpawnObjectAndGet());
            
        }
    }
}
