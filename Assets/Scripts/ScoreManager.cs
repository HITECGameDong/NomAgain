using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] float score;
    [SerializeField] PlayerMovement playerMovement;
    TextMeshPro scoreText;

    void Awake()
    {
        scoreText = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        score += playerMovement.GetCurrentSpeed() * Time.deltaTime;
        scoreText.text = Mathf.Floor(score).ToString();
    }
}
