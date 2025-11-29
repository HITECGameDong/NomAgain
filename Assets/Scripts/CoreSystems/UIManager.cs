using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] ScoreManager scoreUI;
    [SerializeField] UIGameOver gameoverUI;

    void Awake()
    {
        scoreUI.gameObject.SetActive(true);
        gameoverUI.HideUI();
    }

    public void ShowGameOverUI(float lastScore)
    {
        scoreUI.StopScoring();
        gameoverUI.ShowUI(lastScore);
    }
    
}
