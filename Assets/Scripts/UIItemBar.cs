using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIItemBar : UIBasic
{
    void Start()
    {
        player.onItemGet.AddListener(ReduceBarUI);
        UIBar.enabled = false;
        UIBarBG.enabled = false;
    }

    void ReduceBarUI(float duration)
    {
        StartCoroutine(UIEnableCoroutine(duration));
        StartCoroutine(UIBarDurationFilling(duration));
    }

    System.Collections.IEnumerator UIBarDurationFilling(float duration)
    {
        float curTime = 1f;
        while(curTime > 0)
        {
            CamLookingUI();
            curTime -= Time.deltaTime / duration;
            UIBar.fillAmount = curTime;
            yield return null;
        }
    }
}
