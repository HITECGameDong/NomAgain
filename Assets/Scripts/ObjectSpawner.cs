using System.Collections.Generic;
using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Transform nextLocation;
    [SerializeField] float offsetX = 0f;

    // POOLER
    [SerializeField] Transform obstacleParent;
    Queue<GameObject> pool = new Queue<GameObject>();
    const int poolSize = 5;

    [SerializeField] GameManager gameManager;   


    void Start()
    {
        player.onTilePassing.AddListener(SpawnAndPoolingObject);
        //POOLER, create objects first
        SpawnAndPoolingObject();
        SpawnAndPoolingObject();
    }

    void SpawnObject()
    {
        int randomIndex = Random.Range(0, obstacleParent.childCount);

        GameObject objToActive = obstacleParent.GetChild(randomIndex).gameObject;

        while(objToActive.activeSelf)
        {
            // TODO : Fix Lagging. add more blocks in pool
            randomIndex = Random.Range(0, obstacleParent.childCount);
            objToActive = obstacleParent.GetChild(randomIndex).gameObject;
        }

        objToActive.transform.position = nextLocation.position;

        SetActiveRecursive(objToActive);

        pool.Enqueue(objToActive);

        nextLocation.position += new Vector3(offsetX, 0f, 0f);
    }

    void SetActiveRecursive(GameObject other)
    {
        other.SetActive(true);
        foreach(Transform child in other.transform)
        {
            SetActiveRecursive(child.gameObject);
        }
    }

    void SpawnAndPoolingObject()
    {
        if(poolSize <= pool.Count)
        {
            GameObject objToDestroy = pool.Dequeue();
            objToDestroy.SetActive(false);
            SpawnObject();
        }

        else
        {
            SpawnObject();
        }
    }

    public void ResetAndInitializeObjects()
    {
        nextLocation.position = new Vector3(nextLocation.position.x - offsetX*poolSize - gameManager.GetResetLoc(), 0f, 0f);

        for(int i = 0; i < poolSize; i++)
        {
            GameObject passedObj= pool.Dequeue();
            passedObj.transform.position = nextLocation.position;
            nextLocation.position += new Vector3(offsetX, 0f, 0f);
            pool.Enqueue(passedObj);
        }
    }
}
