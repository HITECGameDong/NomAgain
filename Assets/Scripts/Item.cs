using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected ItemType itemType;

    public abstract void GetItem(Player player);

    public ItemType GetItemType()
    {
        return itemType;
    }
}
