using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ObjectSpawner spawner;

    readonly float checkPointX = 50f;


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

    public float GetResetLoc()
    {
        return checkPointX;
    }

    public float GetCurrentLocationForBlock()
    {
        return player.GetCurrentLocation() + player.GetInitLocation();
    }
}   
