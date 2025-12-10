using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIItemBar : UIBasic
{
    [SerializeField] TextMeshProUGUI itemNameText;
    void Start()
    {
        player.onItemGet.AddListener(ReduceBarUI);
        UIBar.enabled = false;
        UIBarBG.enabled = false;
        itemNameText.enabled = false;
    }

    void ReduceBarUI(float duration, string name)
    {
        StartCoroutine(UIBarDurationFilling(duration, name));
    }

    System.Collections.IEnumerator UIBarDurationFilling(float duration, string name)
    {
        itemNameText.text = name;
        itemNameText.enabled = true;

        StartCoroutine(UIEnableCoroutine(duration));
        float curTime = 1f;
        while(curTime > 0)
        {
            curTime -= Time.deltaTime / duration;
            UIBar.fillAmount = curTime;
            yield return null;
        }

        itemNameText.enabled = false;
    }
}
