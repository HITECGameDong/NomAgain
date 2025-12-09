using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] Button returnHomeButton;

    void OnEnable()
    {
        returnHomeButton.onClick.AddListener(() =>
        {
            gameManager.RestartGame();
        });
    }

    public void ShowUI(float lastScore)
    {
        gameObject.SetActive(true);
        finalScoreText.text = Mathf.Floor(lastScore).ToString();
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}

