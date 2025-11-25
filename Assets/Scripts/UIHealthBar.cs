using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : UIBasic
{
    void LateUpdate()
    {
        CamLookingUI();
        UIBar.fillAmount = player.health / player.maxHealth;
    }
}
