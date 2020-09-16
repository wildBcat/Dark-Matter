using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShipController : MonoBehaviour
{
    [Tooltip("Array for all explosions for this ship")]
    public ExplosionController[] allExplosions;

    public void StartExplosion()
    {
        if (allExplosions != null)
        foreach (ExplosionController oneExpl in allExplosions)
            if (oneExpl != null)
                oneExpl.StartExplosion();
    }
}
