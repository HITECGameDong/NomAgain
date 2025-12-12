using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject outlineObject;
    [SerializeField] ParticleSystem brokenParticle;
    bool disableParticle = true;

    void Awake()
    {
        DeactivateOutline();
    }

    public void ActivateOutline()
    {
        outlineObject.SetActive(true);
    }

    public void DeactivateOutline()
    {
        outlineObject.SetActive(false);
    }

    void OnDisable()
    {
        if(disableParticle)
        {
            disableParticle = false;
            return;    
        }

        if(brokenParticle != null)
        {
            Instantiate(brokenParticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f)));
        }
    }
}
