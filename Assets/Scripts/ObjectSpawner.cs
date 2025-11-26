using System.Collections.Generic;
using NUnit.Framework.Internal.Commands;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Transform nextLocForGround;
    // TODO : seperate offset for ground / object
    [SerializeField] float offsetX = 0f;

    // POOLER
    [SerializeField] Transform objectParent;
    Queue<GameObject> spawnedObjPool = new Queue<GameObject>();
    Dictionary<ObjectName, Queue<GameObject>> baseObjPool = new Dictionary<ObjectName, Queue<GameObject>>();
    // TODO : 동전이 한 화면에 몇개까지 있을까, 그거 기반으로 Pool Size 결정할 것.
    readonly int spawnedObjPoolSize = 20;
    readonly int baseObjPoolSize = 7;
    readonly float initObjStartXPos = 20f;
    float objSpawnerCurXPos;


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

    public void SpawnObject()
    {
        SpawnObject(ObjectType.OBSTACLE);
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

        // TODO : separate obs / item spawn
        else if(objType == ObjectType.OBSTACLE || objType == ObjectType.ITEM)
        {
            if(spawnedObjPool.Count >= baseObjPoolSize || spawnedObjPool.Count >= spawnedObjPoolSize)
            {
                // if : spawnPoolSize > baseObjPoolSize, then : for(spPSize-bPSize) spawnedPool -> dequeue(). => disappear infront of player?
                spawnedObjPool.Dequeue().SetActive(false);
            }

            SpawnableObjectSO toSpawnSO = GetRandomObjectSO();
            GameObject objToSpawn;
            do
            {
                objToSpawn = baseObjPool[toSpawnSO.objectName].Dequeue();
                baseObjPool[toSpawnSO.objectName].Enqueue(objToSpawn);
            }
            while(objToSpawn.activeSelf);

            spawnedObjPool.Enqueue(objToSpawn);
            objToSpawn.transform.position = new Vector3(objSpawnerCurXPos, 0f, 0f);
            objSpawnerCurXPos += Random.Range(25f, 30f);
            objToSpawn.SetActive(true);
        }
    }

    void SetActiveRecursive(GameObject other)
    {
        other.SetActive(true);
        foreach(Transform child in other.transform)
        {
            SetActiveRecursive(child.gameObject);
        }
    }

    // TODO : 25-11-26 jin - object spawn logic 변경, 수정예정
    public void ResetAndInitializeObjects()
    {
        nextLocForGround.position = new Vector3(nextLocForGround.position.x - offsetX*groundPoolSize - gameManager.GetResetLoc(), 0f, 0f);

        for(int i = 0; i < groundPoolSize; i++)
        {
            GameObject passedObj= groundPool.Dequeue();
            passedObj.transform.position = nextLocForGround.position;
            groundPool.Enqueue(passedObj);

            nextLocForGround.position += new Vector3(offsetX, 0f, 0f);
        }

        // for(int i = 0; i < spawnedObjPoolSize; i++)
        // {
        //     GameObject tempDequeueObj = spawnedObjPool.Dequeue();
        //     nextLocForObj.position = new Vector3(tempDequeueObj.transform.position.x - gameManager.GetResetLoc(), 0f, 0f);
        //     spawnedObjPool.Enqueue(tempDequeueObj);
        // }
    }

    public void BlockPoolInitialize()
    {
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

        // Instaitiate Spawnable Object to Pool
        foreach (SpawnableObjectSO toSpawnSO in spawnableList)
        {
            baseObjPool[toSpawnSO.objectName] = new Queue<GameObject>();
            for(int i = 0 ; i < baseObjPoolSize; i++)
            {
                GameObject spawnedObject = Instantiate(toSpawnSO.itemPrefab, objectParent);
                spawnedObject.SetActive(false);
                baseObjPool[toSpawnSO.objectName].Enqueue(spawnedObject);
            }
        }

        objSpawnerCurXPos = initObjStartXPos;
        for(int i = 0 ; i < 3 ; i++)
        {
            SpawnObject(ObjectType.ITEM);
        }
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
