using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ScoreManager scoreUI;
    [SerializeField] UIGameOver gameoverUI;
    [SerializeField] UIGameClear gameclearUI;
    [SerializeField] UIWeaponList weaponListUI;
    [SerializeField] UIHome homeUI;
    [SerializeField] UISetting settingUI;

    void Start()
    {
        scoreUI.gameObject.SetActive(false);
        weaponListUI.HideUI();
        gameclearUI.HideUI();
        gameoverUI.HideUI();
        settingUI.gameObject.SetActive(false);
    }

    public void GameStartPressed()
    {
        GameUIInit();
        gameManager.GameStart();
    }

    void GameUIInit()
    {
        scoreUI.gameObject.SetActive(true);
        weaponListUI.ShowUI();
    }

    public void ShowGameOverUI(float lastScore)
    {
        scoreUI.StopScoring();
        weaponListUI.HideUI();
        gameoverUI.ShowUI(lastScore);
    }

    public void ShowGameClearUI(float lastScore)
    {
        scoreUI.StopScoring();
        weaponListUI.HideUI();
        gameclearUI.ShowUI(lastScore);
    }

    public void UpdateWeaponUI(Weapon weapon)
    {
        weaponListUI.UpdateWeaponUI(weapon);
    }
    
    public void SettingsPressed()
    {
        settingUI.gameObject.SetActive(true);   
    }

    public void HomePressed()
    {
        homeUI.gameObject.SetActive(true);
    }
}
