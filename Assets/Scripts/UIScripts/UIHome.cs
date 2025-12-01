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
        Application.targetFrameRate = 60;
        
        playButton.onClick.AddListener(() =>
        {
           SceneManager.LoadScene(1); 
        });

        settingsButton.onClick.AddListener(() =>
        {
           SceneManager.LoadScene(2); 
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

