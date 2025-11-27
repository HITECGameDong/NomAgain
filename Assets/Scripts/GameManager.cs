using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    // COMPOENENTS FROM EDITOR
    [SerializeField] Player player;
    [SerializeField] ObjectSpawner spawner;
    [SerializeField] ScoreManager scoreManager;

    // CONSTANTS
    readonly float checkPointX = 200000f;
    
    // VARS
    [Range(1f, 2f)] [SerializeField] float difficultyMultiply = 1.1f;
    [Range(1f, 10f)][SerializeField] float spawnTimeSec = 5f;
    int tilePassCount = 0;
    float spawnTimer = 0f;
    float curTimeScale = 1f;


    // 25-11-27 TODO-jin : Player, Spawner, ScoreManager 등록되었는지 캐치하기
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
        Time.timeScale = curTimeScale;
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

    void IncreaseDifficulty()
    {
        // TODO : change Theme
        scoreManager.GetTilePassBonus();
        scoreManager.DisplayDifficultyUp();

        curTimeScale *= difficultyMultiply;
        Time.timeScale = curTimeScale;
    }
}   
