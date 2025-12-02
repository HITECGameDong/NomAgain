using UnityEngine;

public class CamMovement : MonoBehaviour
{
    Transform camTransform;
    [SerializeField] Player player;
    [SerializeField] Vector3 camPosOffset;
    [SerializeField] Vector3 camAngleOffset;

    void Awake()
    {
        camTransform = GetComponent<Transform>();
        camTransform.eulerAngles = camAngleOffset;
    }

    void LateUpdate()
    {
        camTransform.position = player.transform.position + camPosOffset;
    }

    public Transform GetCamTransform()
    {
        return camTransform;
    }
}
