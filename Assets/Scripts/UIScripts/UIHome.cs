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
           uiManager.SettingsPressed();
           gameObject.SetActive(false);
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

