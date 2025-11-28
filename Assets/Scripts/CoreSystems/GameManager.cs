using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    // COMPOENENTS FROM EDITOR
    [SerializeField] Player player;
    [SerializeField] ObjectSpawner spawner;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] GameOverUI gameOverUI;

    // CONSTANTS
    readonly float checkPointX = 200000f;
    
    // VARS
    [Range(1f, 2f)] [SerializeField] float difficultyMultiply = 1.1f;
    [Range(4f, 10f)][SerializeField] float defaultSpawnTimeSec;
    [Range(0.5f, 2f)][SerializeField] float minSpawnTimeSec = 1f;
    float curSpawnTimeSec;
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

        SpawnTimerSet(defaultSpawnTimeSec);
        curTimeScale = Time.timeScale;

        gameOverUI.HideUI();
    }

    void FixedUpdate()
    {
        SpawnTimeSetBySpeed();
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
        // 25/11/28 TODO-jin : GAME OVER DISPLAY로 바꾸기
        scoreManager.StopScoring();

        gameOverUI.ShowUI(scoreManager.GetScore());
    }

    void CheckTilePass()
    {
        tilePassCount++;

        if((tilePassCount > 0) && ((tilePassCount % 5) == 0))
        {
            IncreaseDifficulty();
        }
    }

    void SpawnTimerSet(float spawnTime)
    {
        curSpawnTimeSec = spawnTime;
    }

    void SpawnTimeSetBySpeed()
    {
        float speedMult = player.GetCurrentSpeed() / Mathf.Max(0.01f, player.GetBaseSpeed());
        
        // DIV BY 0 방지
        if(speedMult <= 0.01f) return;
        
        float nextSpawnTime = Mathf.Max(defaultSpawnTimeSec / curTimeScale / speedMult,  minSpawnTimeSec);
        SpawnTimerSet(nextSpawnTime);
    }

    void SpawnTimerRun()
    {
        spawnTimer += Time.fixedDeltaTime;
        if(spawnTimer >= curSpawnTimeSec)
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
