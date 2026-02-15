using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameClear : MonoBehaviour
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
