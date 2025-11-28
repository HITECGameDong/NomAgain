using UnityEngine;

public class KillObjectLine : MonoBehaviour
{
    //꼭 Rigidbody 를 달아서 Player와 구분된 Trigger가 되도록 한다. 
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacles"))
        {
            other.GetComponentInParent<Obstacle>().gameObject.SetActive(false);
        }
    }
}
