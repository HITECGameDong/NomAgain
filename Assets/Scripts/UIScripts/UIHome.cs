using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHome : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button ExitButton;

    void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
           SceneManager.LoadScene(1); 
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

