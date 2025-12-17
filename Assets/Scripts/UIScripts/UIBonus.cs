using Unity.VisualScripting;
using UnityEngine;

public class UIBonus : MonoBehaviour
{
   [SerializeField] GameManager gameManager;
   [SerializeField] Animator outlineAnimator;

   void OnEnable()
    {
        gameManager.onDifficultyUp.AddListener(ShowBonusOutlineUI);
    }

    void OnDisable()
    {
        gameManager.onDifficultyUp.RemoveListener(ShowBonusOutlineUI);    
    }

    void ShowBonusOutlineUI()
    {
        StartCoroutine(BonusAnimActivateCoroutine());
    }

    System.Collections.IEnumerator BonusAnimActivateCoroutine()
    {
        outlineAnimator.SetBool("gotBonus", true);
        yield return new WaitForSecondsRealtime(1.5f);
        outlineAnimator.SetBool("gotBonus", false);
    }

}
