using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // EVENTS
    public UnityEvent onDifficultyUp;
    public UnityEvent onObstacleBroken;


    // COMPOENENTS FROM EDITOR
    [SerializeField] Player player;
    [SerializeField] ObjectSpawner spawner;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] UIManager uiManager;

    // CONSTANTS
    readonly float checkPointX = 200000f;
    
    // VARS
    [Range(1f, 2f)] [SerializeField] float difficultyMultiply = 1.1f;
    [Range(4f, 10f)][SerializeField] float defaultSpawnTimeSec;
    [Range(0.5f, 2f)][SerializeField] float minSpawnTimeSec = 1f;
    float curSpawnTimeSec;
    int tilePassCount = 0;
    bool isSpawnTimerRunning = true;
    float spawnTimer = 0f;
    float curTimeScale = 1f;
    bool isGameStarted = false;
    int difficulty = 0;


    // 25-11-27 TODO-jin : Player, Spawner, ScoreManager 등록되었는지 캐치하기
    void Start()
    {
        Application.targetFrameRate = 60;

        player.onArrivingCheckpoint.AddListener(ResetAllPosition);
        player.onPlayerDead.AddListener(GameOver);
        player.onTilePassing.AddListener(CheckTilePass);
        player.onWeaponGet.AddListener(UpdateWeaponUI);

        spawner.BlockPoolInitialize();
        player.PlayerInit();

        SpawnTimerSet(defaultSpawnTimeSec);
        curTimeScale = Time.timeScale;
    }

    void FixedUpdate()
    {
        if(!isGameStarted) return;
        if(isSpawnTimerRunning)
        {
            SpawnTimeSetBySpeed();
            SpawnTimerRun();            
        }
        CheckGameClear();
    }

    public void GameStart()
    {
        isGameStarted = true;
        player.PlayerGameStart();
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

    void StopGame()
    {
        player.StopControl();
        isSpawnTimerRunning = false;
    }

    // Player Die Event에서 호출됩니다.
    void GameOver()
    {
        // Player Die Event 호출시 player.stopcontrol() 중복
        StopGame();
        uiManager.ShowGameOverUI(scoreManager.GetScore());
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
        
        float nextSpawnTime = Mathf.Max(defaultSpawnTimeSec / speedMult,  minSpawnTimeSec);
        SpawnTimerSet(nextSpawnTime);
    }

    void SpawnTimerRun()
    {
        spawnTimer += Time.fixedDeltaTime;
        if(spawnTimer >= curSpawnTimeSec)
        {
            spawner.SpawnObject(difficulty);
            spawnTimer = 0f;
        }
    }

    void CheckGameClear()
    {
        if(player.transform.position.x >= 2000f)
        {
            StopGame();
            uiManager.ShowGameClearUI(scoreManager.GetScore());
        }
    }

    void IncreaseDifficulty()
    {
        // TODO : change Theme
        onDifficultyUp.Invoke();

        curTimeScale *= difficultyMultiply;
        difficulty++;
        Time.timeScale = curTimeScale;
    }

    //25-11-28 TODO-jin : UI Manager가 Scene Control한다. DontDestroy에 넣어야함
    // jin : public인이유는 GameOverUI의 버튼 함수할당을 에디터에서 했기때문. private로 죽어도해야겠다 -> 버튼함수할당 코드로 AddLister
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    //25-11-28 TODO-jin : UI Manager가 Scene Control한다. DontDestroy에 넣어야함
    // jin : public인이유는 GameOverUI의 버튼 함수할당을 에디터에서 했기때문. private로 죽어도해야겠다 -> 버튼함수할당 코드로 AddLister
    public void ReturnHome()
    {
        SceneManager.LoadScene(0);
    }

    void UpdateWeaponUI(Weapon weapon)
    {
        uiManager.UpdateWeaponUI(weapon);
    }
}   
