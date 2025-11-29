using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponTile : MonoBehaviour
{
    [SerializeField] Image cooltimeScreen;
    [SerializeField] Image weaponImage;
    [SerializeField] TextMeshProUGUI weaponLevelText;
    
    public Weapon weapon;
    public bool isEmpty {get; private set;} = true;

    void Awake()
    {
        HideUI();
    }

    public void ShowCooltime()
    {
        if(Mathf.Approximately(weapon.cooltime, 0f))
        {
            return;
        }
        StartCoroutine(ShowCooltimeCoroutine(weapon.cooltime));
    }

    System.Collections.IEnumerator ShowCooltimeCoroutine(float duration)
    {
        float curTime = 1f;
        while(curTime > 0)
        {
            curTime -= Time.deltaTime / duration;
            cooltimeScreen.fillAmount = curTime;
            yield return null;
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        if(weapon == null)
        {
            Debug.LogWarning("Weapon Tile UI에서 받은 등록 weapon 정보 없음, AddWeapon() 참조");
            return;
        }

        this.weapon = weapon;
        weaponImage.sprite = weapon.weaponSO.weaponUISprite;
        weaponLevelText.text = weapon.weaponLevel.ToString();
        isEmpty = false;
        ShowUI();
    }

    public void ShowUI()
    {
        weaponLevelText.enabled = true;
        weaponImage.enabled = true;
    }

    public void HideUI()
    {
        weaponLevelText.enabled = false;
        cooltimeScreen.enabled = false;    
    }
}
