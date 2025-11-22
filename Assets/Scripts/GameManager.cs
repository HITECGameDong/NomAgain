using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ObjectSpawner spawner;

    void Start()
    {
        Application.targetFrameRate = 60;

        player.onArrivingCheckpoint.AddListener(ResetAllPosition);
    }

    void ResetAllPosition()
    {
       player.ResetPosition();
       spawner.ResetAndInitializeObjects();
    }
}
