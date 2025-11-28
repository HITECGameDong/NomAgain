using UnityEngine;

// jin: 개별 Spawn Object Prefab에 다는 물건입니다. Monobehaviour 와 IItem을 모두 상속한 Item Class를 만들어주세요. ex)EnergyBooster.cs
public interface IItem
{
    public void GetItem(Player player);
}
