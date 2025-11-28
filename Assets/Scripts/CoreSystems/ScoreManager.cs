using UnityEngine;
using TMPro;
using UnityEditor.Rendering;

enum ScoreDisplayState
{
    SCORE_STOP,
    SCORE,
    LEVEL_UP,
    GAME_OVER,
}

public class ScoreManager : MonoBehaviour
{
    float score;
    [SerializeField] Player player;
    TextMeshPro scoreText;
    ScoreDisplayState curDisplayState = ScoreDisplayState.SCORE;

    // 25-11-27 TODO-jin : Player 가지고 온거 맞는지 에러 캐치
    void Start()
    {
        scoreText = GetComponent<TextMeshPro>();
        // 25-11-28 TODO-jin : player가 직접 event 호출하도록한다. weapon은 Destroy됨
        player.onObstacleBroken.AddListener(OnObstacleBroken);
    }

    void Update()
    {
        switch(curDisplayState)
        {
            case ScoreDisplayState.SCORE:
                {
                    DisplayAndUpdateScore();
                    break;
                }
            case ScoreDisplayState.LEVEL_UP:
                {
                    break;
                }
            case ScoreDisplayState.GAME_OVER:
                {
                    break;
                }
            case ScoreDisplayState.SCORE_STOP:
                {
                    DisplayScoreStop();
                    break;
                }
            // default:
            //     {
            //         DisplayAndUpdateScore();
            //     }
        }
    }

    void DisplayAndUpdateScore()
    {
        score += player.GetCurrentSpeed() * Time.deltaTime;
        scoreText.text = Mathf.Floor(score).ToString();
    }

    void DisplayScoreStop()
    {
        scoreText.text = Mathf.Floor(score).ToString();
    }

    public void StopScoring()
    {
        curDisplayState = ScoreDisplayState.SCORE_STOP;
    }

    void AddScore(float scoreAddition)
    {
        score += scoreAddition;
    }

    public void GetTilePassBonus()
    {
        score += 200;
    }

    public float GetScore()
    {
        return score;
    }

    // GAME MANAGER에 의해, 난이도 상승시 한번 호출됨.
    public void DisplayDifficultyUp()
    {
        StartCoroutine(DisplayDiffucultyUpCoroutine());
    }

    System.Collections.IEnumerator DisplayDiffucultyUpCoroutine()
    {
        curDisplayState = ScoreDisplayState.LEVEL_UP;
        scoreText.text = "Difficulty Up!";
        yield return new WaitForSeconds(1f);
        curDisplayState = ScoreDisplayState.SCORE;
    }

    void OnObstacleBroken()
    {
        AddScore(400f);
    }
}
