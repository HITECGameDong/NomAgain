using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public UnityEvent onPositionReset;
    [SerializeField] Player player;

    void Start()
    {
        Application.targetFrameRate = 60;

        player.onArrivingCheckpoint.AddListener(ResetAllPosition);
    }

    void ResetAllPosition()
    {
        //onPositionReset.Invoke();
    }
}
