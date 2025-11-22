using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] float score;
    [SerializeField] PlayerMovement playerMovement;
    TextMeshPro scoreText;
    bool isScoreCalculatable = true;

    void Awake()
    {
        scoreText = GetComponent<TextMeshPro>();
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
        score += playerMovement.GetCurrentSpeed() * Time.deltaTime;
        scoreText.text = Mathf.Floor(score).ToString();
    }

    public void StopScoring()
    {
        isScoreCalculatable = false;
    }
}
