using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [Tooltip("Sets the amount of damage game object deals on collision with another object. The higher the number, the more damage it assigns during each collision.")]
    [SerializeField] int damage = default;

    [Tooltip("Sets The amount of health game object has. Any damage assigned to the game object will reduce this number. The higher the number, the more it will take to kill the game object.")]
    [SerializeField] int health = default;

    [Tooltip("Sets the color the game object will flash when damage is assigned.")]
    [SerializeField] Color collideColor = default;

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
        health -= baseObjectController.GetDamage();
        if (health <= 0)
        {
            Die();
        }
        else
        {
            if(!CompareTag("Asteroid"))
            {
                StartCoroutine(Flash());
            }
        }
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

    private void Die()
    {
        ExplosionController[] allExplosions = GameObject.FindObjectsOfType<ExplosionController>();
        foreach (ExplosionController oneExpl in allExplosions)
            oneExpl.StartExplosion();
    }


}
