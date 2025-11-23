using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] float score;
    [SerializeField] Player player;
    TextMeshPro scoreText;
    bool isScoreCalculatable = true;

    void Start()
    {
        scoreText = GetComponent<TextMeshPro>();
        player.equippedWeapon.onObstacleBroken.AddListener(GetScore);
    }

    void Update()
    {
        if(isScoreCalculatable)
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

    void GetScore()
    {
        score += 40f;
    }
}
