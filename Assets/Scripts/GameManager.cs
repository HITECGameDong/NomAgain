using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ObjectSpawner spawner;
    [SerializeField] ScoreManager scoreManager;

    [SerializeField] float spdAddForEachDifficulty = 2f;
    
    readonly float checkPointX = 200000f;

    int tilePassCount = 0;

    [SerializeField] float spawnTimeSec = 5f;
    [SerializeField] float spawnTimeReduceSec = 0.5f;
    float spawnTimer = 0f;


    void Start()
    {
        Application.targetFrameRate = 60;

        player.onArrivingCheckpoint.AddListener(ResetAllPosition);
        player.onPlayerDead.AddListener(GameOver);
        player.onTilePassing.AddListener(CheckTilePass);

        spawner.BlockPoolInitialize();
    }

    void FixedUpdate()
    {
        SpawnTimerRun();
    }

    void ResetAllPosition()
    {
        Time.timeScale = 0f;
        player.ResetPosition();
        spawner.ResetAndInitializeObjects();
        Time.timeScale = 1f;
    }

    public float GetResetLoc()
    {
        return checkPointX;
    }

    void GameOver()
    {
        player.Kill();
        scoreManager.StopScoring();
    }

    void CheckTilePass()
    {
        tilePassCount++;

        if((tilePassCount > 0) && ((tilePassCount % 5) == 0))
        {
            IncreaseDifficulty();
        }
    }

    void SpawnTimerRun()
    {
        spawnTimer += Time.fixedDeltaTime;
        if(spawnTimer >= spawnTimeSec)
        {
            spawner.SpawnObject();
            spawnTimer = 0f;
        }
    }

    void ReduceSpawnTime(float reduceTime)
    {
        spawnTimeSec = Mathf.Max(0f, spawnTimeSec - reduceTime);
    }

    void IncreaseDifficulty()
    {
        // TODO : change Theme
        scoreManager.GetTilePassBonus();
        scoreManager.DisplayDifficultyUp();
        player.IncreaseDefaultSpeed(spdAddForEachDifficulty);
        ReduceSpawnTime(spawnTimeReduceSec);
        Debug.Log("Gain Diff");
    }
}   
