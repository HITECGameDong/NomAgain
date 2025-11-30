using Unity.Mathematics;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] ParticleSystem itemGetParticle;
    bool particleDisable = true;

    public abstract void GetItem(Player player);
    protected virtual void OnDisable()
    {
        // 최초 pool Init시 particle 진행X
        if(particleDisable)
        {
            particleDisable = false;
            return;
        }

        if(itemGetParticle != null)
        {
            Instantiate(itemGetParticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f)));
        }
    } 
}
