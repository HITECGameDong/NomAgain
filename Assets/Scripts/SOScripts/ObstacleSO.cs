using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSO", menuName = "SOs/Obstacle")]
public class ObstacleSO : SpawnableObjectSO
{
    public ParticleSystem onBrokenParticle;
}
