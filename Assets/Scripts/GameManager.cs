using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ObjectSpawner spawner;

    readonly float checkPointX = 200000f;


    void Start()
    {
        Application.targetFrameRate = 60;

        player.onArrivingCheckpoint.AddListener(ResetAllPosition);
    }

    void ResetAllPosition()
    {
        Time.timeScale = 0f;
        player.ResetPosition();
        spawner.ResetAndInitializeObjects();
        Time.timeScale = 1f;
    }

    public float GetResetLoc()
    {
        return checkPointX;
    }
}   
