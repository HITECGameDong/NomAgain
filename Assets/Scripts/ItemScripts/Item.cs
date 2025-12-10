using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    bool particleDisable = true;

    public virtual void GetItem(Player player)
    {
        itemSO.GetItem(player);
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        // 최초 pool Init시 particle 진행X
        if(particleDisable)
        {
            particleDisable = false;
            return;
        }

        if(itemSO.itemGetParticle != null)
        {
            Instantiate(itemSO.itemGetParticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f)));
        }
    } 
}
