using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [Tooltip("GameObject which contains only ship body that should be delete during explosion")]
    public GameObject mainBody;
    [Tooltip("GameObject which contains mainBody GameObject AND all explosion(s)")]
    public GameObject mainContainer = null;

    public void ExplosionComplete()
    {
        if (mainContainer != null)
        {
            Destroy(mainContainer);
        }

    }

    public void DestroyMainBody()
    {
        if (mainBody != null)
            Destroy(mainBody);
    }

    public void StartExplosion()
    {
        GetComponent<Animator>().SetTrigger("boom");
    }
}
