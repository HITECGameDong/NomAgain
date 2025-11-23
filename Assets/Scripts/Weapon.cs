using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] BoxCollider range;
    protected GameObject currentTarget = null;

    void Awake()
    {
        range = GetComponent<BoxCollider>();
    }

    public abstract void Attack();

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("HitLine"))
        {
            currentTarget = other.GetComponentInParent<Obstacle>().gameObject; 
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("HitLine"))
        {
            currentTarget = null;
        }
    }
}
