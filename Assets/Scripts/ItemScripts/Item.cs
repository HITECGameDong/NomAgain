using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;

    public virtual void GetItem(Player player)
    {
        itemSO.GetItem(player);
        DestroyMyself();
    }

    public void DestroyMyself()
    {
        if(itemSO.itemGetParticle != null)
        {
            Instantiate(itemSO.itemGetParticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f)));
        }      

        gameObject.SetActive(false);  
    } 
}
