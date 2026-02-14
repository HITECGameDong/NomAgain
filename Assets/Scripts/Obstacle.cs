using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject outlineObject;
    [SerializeField] ParticleSystem brokenParticle;

    void Awake()
    {
        DeactivateOutline();
    }

    void OnDisable()
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

    public void DestroyObstacle()
    {
        if(brokenParticle != null)
        {
            Instantiate(brokenParticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f)));
        }

        gameObject.SetActive(false);
    }
    public void DestroyObstacle(Weapon weapon)
    {
        if(weapon.weaponSO.obstacleBrokenParticle != null)
        {
            Instantiate(weapon.weaponSO.obstacleBrokenParticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f)));
        }

        else if(brokenParticle != null)
        {
            Instantiate(brokenParticle, transform.position, Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f)));
        }

        gameObject.SetActive(false);
    }
}
