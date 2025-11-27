using UnityEngine;

public class RocketBooster : MonoBehaviour, IItem
{
    [SerializeField] float speedAddition = 30f;
    [SerializeField] float duration = 2f;

    // 25-11-27 TODO-jin : addition / duration err catch넣기
    public void GetItem(Player player)
    {
       player.GetRocketBoost(speedAddition, duration);
       gameObject.SetActive(false);
    }
}
