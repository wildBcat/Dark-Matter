using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [SerializeField] int damage = default;

    [SerializeField] int health = default;

    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
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
    }

    private void Die()
    {
        ExplosionController[] allExplosions = GameObject.FindObjectsOfType<ExplosionController>();
        foreach (ExplosionController oneExpl in allExplosions)
            oneExpl.StartExplosion();
    }
}
