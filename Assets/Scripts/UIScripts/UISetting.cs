using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    //기본틀
    [SerializeField] Button backbutton;

    void Awake()
    {
        
        Application.targetFrameRate = 60;

        backbutton.onClick.AddListener(() =>
        {
           SceneManager.LoadScene(0); 
        });

    }

}
