using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] ParticleSystem brokenParticle;
    bool disableParticle = true;

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
