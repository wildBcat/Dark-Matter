using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [Header("Object Stats")]
    [Tooltip("Sets the amount of damage game object deals on collision with another object. " +
        "The higher the number, the more damage it assigns during each collision.")]
    [SerializeField] int damage = default;
    [Tooltip("Sets The amount of health game object has. Any damage assigned to the game object will reduce this number. " +
        "The higher the number, the more it will take to kill the game object.")]
    [SerializeField] int health = default;

    [Header("Damage Settings")]
    [Tooltip("Sets whether the object will flash when damage is received. " +
        "The object will also become invunerable during the duration of the flashing.")]
    [SerializeField] bool damageFlicker = default;
    [Range(5, 40)]
    [Tooltip("Sets the number of frames the Damage Flicker will last once triggered.")]
    public int FlickerDuration = 6;
    [Tooltip("Sets the color the sprite will flash when damage is assigned. " +
        "If this isn't set then the mat will take on the color of the sprite.")]
    [SerializeField] Color flickerColor = default;

    [Header("Destruction Settings")]
    [Tooltip("Array for all explosions for this ship")]
    public ExplosionController[] explosions;
    
    SpriteRenderer[] renderers = default;

    Color[] originalColors = default;

    private bool isInvincible = default;

    private void Start()
    {
        // Loads the array with each child sprite's sprite renderer
        renderers = GetComponentsInChildren<SpriteRenderer>();
        renderers = renderers.Where(child => child.tag == "Parts").ToArray();

        // Sets the number of original colors to load
        originalColors = new Color[renderers.Length];

        // Loads the array with each child sprite's sprite renderer color
        for (int i = 0; i < originalColors.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }
    }

    public int GetDamage()
    {
        // Assigns damage
        return damage;
    }
    
    public void ProjectileHit()
    {
        // Destroys projectile on impact
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collidingGameObject)
    {
        // Checks to see if the colliding object uses the BaseObjectController script. If it does, it processes a hit
        BaseObjectController baseObjectController = collidingGameObject.gameObject.GetComponent<BaseObjectController>();

        if (!baseObjectController)
        {
            return;
        }

        ProcessHit(baseObjectController);
    }

    private void ProcessHit(BaseObjectController baseObjectController)
    {
        // Prevents damage to the object while the object is invincible
        if (isInvincible) return;

        // Decreased the health of the game object by the collinding object's damage amount
        health -= baseObjectController.GetDamage();

        // If the received damage reduces the game objects health to 0 or less it destroys it, otherwise it initiates a damage flash
        if (health <= 0)
        {
            health = 0;
            Die();
            return;
        }
        if (damageFlicker)
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
        else
        {
            return;
        }
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;

        bool flicker = false;
        for (int i = 0; i < FlickerDuration * 2; i++)
        {
            flicker = !flicker;

            if (flicker)
            {
                foreach (SpriteRenderer r in renderers)
                {
                    // Sets each sprite in the renderers array to the color set in the collideColor variable
                    r.color = flickerColor;
                }
            }

            else
            {
                for (int l = 0; l < originalColors.Length; l++)
                {
                    // Returns each sprite in the renderers array to the color set in the originalColors variable
                    renderers[l].color = originalColors[l];
                }
            }
            yield return null;
        };

        for (int l = 0; l < originalColors.Length; l++)
        {
            // Returns each sprite in the renderers array to the color set in the originalColors variable
            renderers[l].color = originalColors[l];
        }
        isInvincible = false;
    }

    private void Die()
    {
        // Initiates all the explosions attached to the object
        if (explosions != null)
            foreach (ExplosionController oneExpl in explosions)
                if (oneExpl != null)
                    oneExpl.StartExplosion();
    }
}
