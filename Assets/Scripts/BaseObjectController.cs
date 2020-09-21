using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [SerializeField] int damage = default;

    [SerializeField] int health = default;

    [SerializeField] Color collideColor = default;

    Color normalColor;

    private Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;


    SpriteRenderer[] renderers;
    Color[] originalColors;
    private void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < originalColors.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }

        sr = GetComponent<SpriteRenderer>();

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
            matDefault = sr.GetComponent<SpriteRenderer>().material;
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public void ProjectileHit()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collidingGameObject)
    {
        BaseObjectController baseObjectController = collidingGameObject.gameObject.GetComponent<BaseObjectController>();
        if (!baseObjectController)
        {
            return;
        }
        ProcessHit(baseObjectController);
    }

    private void ProcessHit(BaseObjectController baseObjectController)
    {
        StartCoroutine(Flash());
        
        health -= baseObjectController.GetDamage();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ExplosionController[] allExplosions = GameObject.FindObjectsOfType<ExplosionController>();
        foreach (ExplosionController oneExpl in allExplosions)
            oneExpl.StartExplosion();
    }

    IEnumerator Flash()
    {
        for (int j = 0; j < 5; j++)
        {
            foreach (SpriteRenderer r in renderers)
            {
                r.color = collideColor;
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material = matWhite;
            }
            yield return new WaitForSeconds(.1f);
            for (int i = 0; i < originalColors.Length; i++)
            {
                renderers[i].color = originalColors[i];
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material = matDefault;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
