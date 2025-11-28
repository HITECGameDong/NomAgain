using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] Vector3 BGPosOffset;
    private MeshRenderer render;
    private float offset = 0f;
    [SerializeField] Player player;
    [SerializeField] float speedoffset;
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        render = GetComponent <MeshRenderer>();

        // speed offset 0인지 확인, Div by 0 방지.
        if (Mathf.Approximately(speedoffset, 0f))
        {
            Debug.LogWarning("Background Scroller의 Speed Offset이 0입니다, 기본 설정 100으로 적용");
            speed = 1/100f;
        }
        else
        {
            speed = 1 / speedoffset;
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + BGPosOffset.x, BGPosOffset.y, BGPosOffset.z);
    }

    // Update is called once per frame
    void Update()
    {
        offset += player.GetCurrentSpeed() * Time.deltaTime * speed;
        render.material.mainTextureOffset = new Vector2( offset , 0);
    }
}
