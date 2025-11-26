using UnityEngine;

public class Background : MonoBehaviour
{
    Vector3 BGPosOffset;
    Transform BGTransform;
    private MeshRenderer render;
    private float offset;
    [SerializeField] Player player;
    [SerializeField] float speedoffset;
    [SerializeField] float offsetX = 8f;    
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        render = GetComponent <MeshRenderer>();
        speed = 1 / speedoffset;
        BGTransform = GetComponent<Transform>();
        BGPosOffset = new Vector3(offsetX, 1f, 4f);
    }

    void LateUpdate()
    {
        BGTransform.position = player.transform.position + BGPosOffset;
    }

    // Update is called once per frame
    void Update()
    {
        offset += player.GetCurrentSpeed() * Time.deltaTime * speed;
        render.material.mainTextureOffset = new Vector2( offset , 0);
    }

    public Transform GetBGTransform()
    {
        return BGTransform;
    }
}
