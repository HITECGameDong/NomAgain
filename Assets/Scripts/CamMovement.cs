using UnityEngine;

public class CamMovement : MonoBehaviour
{
    Transform camTransform;
    [SerializeField] Player player;
    [SerializeField] float offsetX = 8f;    
    [SerializeField] float offsetZ = -15f;
    Vector3 camPosOffset;


    void Awake()
    {
        camTransform = GetComponent<Transform>();
        camPosOffset = new Vector3(offsetX, 0f, offsetZ);
    }

    void LateUpdate()
    {
        camTransform.position = player.transform.position + camPosOffset;
    }

    
}
