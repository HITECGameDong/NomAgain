using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

// 획득 가능한 Spawn Item을 정의.
public abstract class ItemSO : SpawnableObjectSO
{
    public ParticleSystem itemGetParticle;

    public abstract void GetItem(Player player);
}
