using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ScoreManager scoreUI;
    [SerializeField] UIGameOver gameoverUI;
    [SerializeField] UIWeaponList weaponListUI;
    [SerializeField] UIHome UIHome;

    void Start()
    {
        scoreUI.gameObject.SetActive(false);
        weaponListUI.HideUI();
        gameoverUI.HideUI();
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

    public void UpdateWeaponUI(Weapon weapon)
    {
        weaponListUI.UpdateWeaponUI(weapon);
    }
    
}
