using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] ScoreManager scoreUI;
    [SerializeField] UIGameOver gameoverUI;
    [SerializeField] UIWeaponList weaponListUI;

    void Start()
    {
        scoreUI.gameObject.SetActive(true);
        gameoverUI.HideUI();
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
