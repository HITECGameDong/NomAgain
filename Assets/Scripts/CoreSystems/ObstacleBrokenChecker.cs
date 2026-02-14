using UnityEngine;

public class ObstacleBrokenChecker : MonoBehaviour
{
    public void DisableMyselfWhenEveryChildDead()
    {
        bool isOkToDisableParent = true;
        for(int i = 0; i < this.transform.childCount; i++)
        {
            if(this.transform.GetChild(i).gameObject.activeSelf)
            {
                isOkToDisableParent = false;
                break;
            }
        }

        if(isOkToDisableParent)
        {
            this.gameObject.SetActive(false);
        }
    }
}
