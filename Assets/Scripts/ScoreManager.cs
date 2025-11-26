using UnityEngine;
using TMPro;
using UnityEditor.Rendering;

public class ScoreManager : MonoBehaviour
{
    float score;
    [SerializeField] Player player;
    TextMeshPro scoreText;
    bool isScoreCalculatable = true;
    bool isScoreDisplayable = true;

    void Start()
    {
        scoreText = GetComponent<TextMeshPro>();
        player.equippedWeapon.onObstacleBroken.AddListener(AddScore);
    }

    void Update()
    {
        if(isScoreCalculatable && isScoreDisplayable)
        {
            ScoreUpdate();
        }
    }

    void ScoreUpdate()
    {
        score += player.GetCurrentSpeed() * Time.deltaTime;
        scoreText.text = Mathf.Floor(score).ToString();
    }

    public void StopScoring()
    {
        isScoreCalculatable = false;
    }

    void AddScore()
    {
        score += 40f;
    }

    public void GetTilePassBonus()
    {
        score += 200;
    }

    public float GetScore()
    {
        return score;
    }

    public void DisplayDifficultyUp()
    {
        StartCoroutine(DisplayDiffucultyUpCoroutine());
    }

    System.Collections.IEnumerator DisplayDiffucultyUpCoroutine()
    {
        isScoreCalculatable = false;
        scoreText.text = "Difficulty Up!";
        yield return new WaitForSeconds(1f);
        isScoreCalculatable = true;
    }
}
