using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;

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

