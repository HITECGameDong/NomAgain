using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] Button backbutton;

    void Awake()
    {
        backbutton.onClick.AddListener(() =>
        {
           uiManager.HomePressed();
           gameObject.SetActive(false);
        });
    }

}
