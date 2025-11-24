using System.Collections.Generic;
using NUnit.Framework.Internal.Commands;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Transform nextLocForGround;
    [SerializeField] Transform nextLocForObj;
    // TODO : seperate offset for ground / object
    [SerializeField] float offsetX = 0f;

    // POOLER
    [SerializeField] Transform obstacleParent;

    Queue<GameObject> pool = new Queue<GameObject>();
    const int poolSize = 10;

    [SerializeField] GameManager gameManager;   

    [SerializeField] List<SpawnableObjectSO> spawnableList;
    [SerializeField] SpawnableObjectSO GroundSO;
    [SerializeField] Transform groundPoolParentTransform;
    Queue<GameObject> groundPool = new Queue<GameObject>();
    readonly int groundPoolSize = 4;



    void Start()
    {
        player.onTilePassing.AddListener(SpawnGround);
    }

    void SpawnGround()
    {
        SpawnObject(ObjectType.GROUND);
    }

    void SpawnObject(ObjectType objType)
    {
        if(objType == ObjectType.NONE) return;

        if(objType == ObjectType.GROUND)
        {
            // GROUND SPAWN
            GameObject objToActive = groundPool.Dequeue();
            objToActive.transform.position = nextLocForGround.position;
            objToActive.SetActive(true);

            groundPool.Enqueue(objToActive);

            nextLocForGround.position += new Vector3(offsetX, 0f, 0f);    
        }

        else if(objType == ObjectType.OBSTACLE || objType == ObjectType.ITEM)
        {
            
        }

        // OBJECTS SPAWN

        // int randomIndex = Random.Range(0, obstacleParent.childCount);

        // GameObject objToActive = obstacleParent.GetChild(randomIndex).gameObject;

        // while(objToActive.activeSelf)
        // {
        //     // TODO : Fix Lagging. add more blocks in pool
        //     randomIndex = Random.Range(0, obstacleParent.childCount);
        //     objToActive = obstacleParent.GetChild(randomIndex).gameObject;
        // }

        // objToActive.transform.position = nextLocation.position;

        // SetActiveRecursive(objToActive);

        // pool.Enqueue(objToActive);

        // nextLocation.position += new Vector3(offsetX, 0f, 0f);
    }

    void SetActiveRecursive(GameObject other)
    {
        other.SetActive(true);
        foreach(Transform child in other.transform)
        {
            SetActiveRecursive(child.gameObject);
        }
    }

    public void ResetAndInitializeObjects()
    {
        nextLocForGround.position = new Vector3(nextLocForGround.position.x - offsetX*poolSize - gameManager.GetResetLoc(), 0f, 0f);
        //nextLocForObj.position = new Vector3(nextLocForObj.position.x - offsetX*poolSize - gameManager.GetResetLoc(), 0f, 0f);

        Vector3 nextLocCpyForGround = nextLocForGround.position;
        //Vector3 nextLocCpyForObj = nextLocForObj.position;

        for(int i = 0; i < groundPoolSize; i++)
        {
            GameObject passedObj= groundPool.Dequeue();
            passedObj.transform.position = nextLocCpyForGround;
            groundPool.Enqueue(passedObj);

            nextLocCpyForGround += new Vector3(offsetX, 0f, 0f);
            nextLocForGround.position += new Vector3(offsetX, 0f, 0f);
        }

        // for(int i = 0; i < poolSize; i++)
        // {
        //     GameObject passedObj= pool.Dequeue();
        //     passedObj.transform.position = nextLocCpyForObj;
        //     pool.Enqueue(passedObj);

        //     nextLocCpyForObj += new Vector3(offsetX, 0f, 0f);
        //     nextLocOfObj.position += new Vector3(offsetX, 0f, 0f);
        // }
    }

    public void BlockPoolInitialize()
    {
        //POOLER, create objects first
        // SpawnAndPoolingObject();
        // SpawnAndPoolingObject();
        
        // Instantiate Ground to Pool
        for(int i = 0; i < groundPoolSize ; i++)
        {
            GameObject spawnedGround = Instantiate(GroundSO.itemPrefab, nextLocForGround.position, 
                Quaternion.identity, groundPoolParentTransform);

            if(i < 2)
            {
                spawnedGround.SetActive(false);
            }
            else
            {
                nextLocForGround.position += new Vector3(offsetX, 0f, 0f);
            }
                groundPool.Enqueue(spawnedGround);
        }

        // // Instaitiate Spwanable Object to Pool
        // for(int i = 0 ; i < poolSize ; i++)
        // {
        //     GameObject spawnedObject = Instantiate(GetRandomObjectSO().itemPrefab, obstacleParent);
        //     spawnedObject.SetActive(false);
        // }
    }

    SpawnableObjectSO GetRandomObjectSO()
    {
        float total = 0f;
        for (int i = 0; i < spawnableList.Count; i++)
            total += Mathf.Max(0f, spawnableList[i].spawnWeight);

        // prevent dev mistake.. will never happen
        if (total <= 0f) return null;

        float r = Random.Range(0f, total);

        float cumulative = 0f;
        for (int i = 0; i < spawnableList.Count; i++)
        {
            cumulative += Mathf.Max(0f, spawnableList[i].spawnWeight);
            if (r <= cumulative)
                return spawnableList[i];
        }

        return spawnableList[^1]; // 부동소수점 안전망...        
    }
}
