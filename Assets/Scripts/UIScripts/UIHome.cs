using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHome : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button ExitButton;

    void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
           uiManager.GameStartPressed();
           gameObject.SetActive(false);
        });

        settingsButton.onClick.AddListener(() =>
        {
           //SceneManager.LoadScene(2); 
        });

        ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        });
    }
}

