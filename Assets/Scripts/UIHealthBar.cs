using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : UIBasic
{
    void LateUpdate()
    {
        UIBar.fillAmount = player.health / player.maxHealth;
    }
}
