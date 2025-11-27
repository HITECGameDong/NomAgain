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
    Dictionary<SpawnableObjectSO, Queue<GameObject>> baseObjPool = new Dictionary<SpawnableObjectSO, Queue<GameObject>>();
    // TODO : 동전이 한 화면에 몇개까지 있을까, 그거 기반으로 Pool Size 결정할 것.
    readonly int baseObjPoolSize = 10;
    readonly float initObjStartXPos = 20f;
    float objSpawnerCurXPos;


    [SerializeField] GameManager gameManager;   

    [SerializeField] List<SpawnableObjectSO> spawnableObjList;
    [SerializeField] SpawnableObjectSO GroundSO;
    [SerializeField] Transform groundPoolParentTransform;
    Queue<GameObject> groundPool = new Queue<GameObject>();
    readonly int groundPoolSize = 4;



    // 25-11-27 TODO-jin : Player / GM 객체 가지고 있는지 에러 캐치할것! 
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
        ChangeSpawnLocByPlayerLoc();

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

        // ? TODO-jin : separate obs / item spawn
        else if(objType == ObjectType.OBSTACLE || objType == ObjectType.ITEM)
        {
            SpawnableObjectSO toSpawnSO = GetRandomObjectSO();
            GameObject objToSpawn;
            GameObject firstQueueObject = baseObjPool[toSpawnSO].Dequeue();
            baseObjPool[toSpawnSO].Enqueue(firstQueueObject);
            objToSpawn = firstQueueObject;

            while(objToSpawn.activeSelf)
            {
                objToSpawn = baseObjPool[toSpawnSO].Dequeue();
                baseObjPool[toSpawnSO].Enqueue(objToSpawn);

                if(firstQueueObject == objToSpawn)
                {
                    toSpawnSO = GetRandomObjectSO();
                }
            }

            objToSpawn.transform.position = new Vector3(objSpawnerCurXPos, 0f, 0f);
            objSpawnerCurXPos += Random.Range(25f, 30f);
            objToSpawn.SetActive(true);
        }
    }

    // 기능: Object Spawner 위치가 플레이어 근처나 뒤에 있으면 앞으로 옮김.
    void ChangeSpawnLocByPlayerLoc()
    {
        float curPlayerPosX = player.transform.position.x;
        if(curPlayerPosX + 25f >= objSpawnerCurXPos)
        {
            objSpawnerCurXPos = curPlayerPosX + Random.Range(25f, 30f);            
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

    // 25-11-26 TODO-jin : - object spawn logic 변경, 수정예정
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

    // 25-11-27 TODO-jin : difficulty 당 다르게 스폰하도록하기.
    // 25-11-27 TODO-jin : spawnableObjList 중복값 있으면 Debug.LogWarn하기(basePool Dict므로 문제는없음)
    public void BlockPoolInitialize()
    {
        // Instantiate Ground to Pool
        for(int i = 0; i < groundPoolSize ; i++)
        {
            GameObject spawnedGround = InstantiateAndGetFromObjectSO(GroundSO, 0, nextLocForGround, groundPoolParentTransform);
            if(i >= 2)
            {
                spawnedGround.SetActive(true);
                nextLocForGround.position += new Vector3(offsetX, 0f, 0f);
            }

            groundPool.Enqueue(spawnedGround);
        }

        // Instaitiate Spawnable Object to Pool
        foreach (SpawnableObjectSO toSpawnSO in spawnableObjList)
        {
            baseObjPool[toSpawnSO] = new Queue<GameObject>();
            for(int i = 0 ; i < baseObjPoolSize; i++)
            {
                baseObjPool[toSpawnSO].Enqueue(InstantiateAndGetFromObjectSO(toSpawnSO, 0, objectParent));
            }
        }

        objSpawnerCurXPos = initObjStartXPos;
        for(int i = 0 ; i < 3 ; i++)
        {
            SpawnObject(ObjectType.ITEM);
        }
    }

    // jin: 누적합 방식 랜덤 뽑기 채용. 백분율 기준 뽑기는 어떻게할지?
    SpawnableObjectSO GetRandomObjectSO()
    {
        if(spawnableObjList.Count <= 0)
        {
            Debug.LogWarning("Object Spawner의 Spawn List에 아무것도 없음, Editor에서 등록하세요");
            return null;
        }

        float total = 0f;
        for (int i = 0; i < spawnableObjList.Count; i++)
            total += Mathf.Max(0f, spawnableObjList[i].spawnWeight);

        // prevent dev mistake.. will never happen
        if (total <= 0f) return null;

        float r = Random.Range(0f, total);

        float cumulative = 0f;
        for (int i = 0; i < spawnableObjList.Count; i++)
        {
            cumulative += Mathf.Max(0f, spawnableObjList[i].spawnWeight);
            if (r <= cumulative)
                return spawnableObjList[i];
        }

        return spawnableObjList[^1]; // 부동소수점 안전망...      
    }

    // 25-11-27 TODO-jin : difficulty 당 spawnable List의 index 배정 및 해당 함수에 기능 적용하기.
    // 기능: 최초 Game Init시 Spawner가 ground pool에 ground obj 생성시 사용함(생성위치포함함수임). 
    // difficulty 적용 관련 따로 함수로 빼서 에러 캐치할수있게.
    GameObject InstantiateAndGetFromObjectSO(SpawnableObjectSO toSpawnSO, int difficulty, Transform spawnLocTransform, Transform parentTransform)
    {
        if(toSpawnSO == null)
        {
            Debug.LogWarning("Object Instantiate시 받은 SpawnableObjectSO가 null임, SO 설정 재확인할것");
            return null;
        }

        GameObject toSpawnObj = Instantiate(toSpawnSO.itemPrefabList[0], spawnLocTransform.position, 
                Quaternion.identity, parentTransform);
        toSpawnObj.SetActive(false);

        return toSpawnObj;
    }

    // 25-11-27 TODO-jin : difficulty 당 spawnable List의 index 배정 및 해당 함수에 기능 적용하기.
    // 기능: 최초 Game Init시 Spawner가 base pool에 오브젝트 생성시 사용함(생성위치미포함함수임). 
    // difficulty 적용 관련 따로 함수로 빼서 에러 캐치할수있게.
    GameObject InstantiateAndGetFromObjectSO(SpawnableObjectSO toSpawnSO, int difficulty, Transform parentTransform)
    {
        if(toSpawnSO == null)
        {
            Debug.LogWarning("Object Instantiate시 받은 SpawnableObjectSO가 null임, SO 설정 재확인할것");
            return null;
        }

        GameObject toSpawnObj = Instantiate(toSpawnSO.itemPrefabList[0], parentTransform);
        toSpawnObj.SetActive(false);
        return toSpawnObj;
    }
}
