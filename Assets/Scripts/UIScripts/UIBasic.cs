using UnityEngine;
using UnityEngine.UI;

public class UIBasic : MonoBehaviour
{
    [SerializeField] protected Player player;
    [SerializeField] protected Image UIBar;
    [SerializeField] protected Image UIBarBG;

    protected System.Collections.IEnumerator UIEnableCoroutine(float duration)
    {
        UIBar.enabled = true;
        UIBarBG.enabled = true;
        yield return new WaitForSeconds(duration);
        UIBar.enabled = false;
        UIBarBG.enabled = false;
    }
}
