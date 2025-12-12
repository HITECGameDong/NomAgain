using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        ChangeToNormalAnim();
    }

    public void ChangeToPunchableAnim()
    {
        animator.SetBool("isPunchable", true);
    }

    public void ChangeToNormalAnim()
    {
        animator.SetBool("isPunchable",false);
    }
}
