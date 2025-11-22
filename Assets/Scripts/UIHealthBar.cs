using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Image healthBar;

    void LateUpdate()
    {
        healthBar.fillAmount = player.health / player.maxHealth;
    }
}
