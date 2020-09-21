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

    private void Start()
    {

        sr = GetComponent<SpriteRenderer>();

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            normalColor = sr.color; // Fix this so it stores all the sprite colors of the chilren in an array, then restores them. 
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
        for (int i = 0; i < 5; i++)
        {
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = collideColor;
                sr.material = matWhite;
            }
            yield return new WaitForSeconds(.1f);
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = normalColor;
                sr.material = matDefault;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
