using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] bool damageFlash = default;

    [Tooltip("Sets the color the sprite will flash when damage is assigned. " +
        "If this isn't set then the mat will take on the color of the sprite.")]
    [SerializeField] Color flashColor = default;

    [Range(1, 10)]
    [SerializeField] private int numberOfFlashes = default;

    [Range(.01f, .2f)]
    [SerializeField] private float flashSpeed;

    [Header("Destruction Settings")]
    [Tooltip("Array for all explosions for this ship")]
    public ExplosionController[] explosions;

    private Material matWhite;    
    private Material matDefault;    
    SpriteRenderer[] renderers;
    Color[] originalColors;

    private bool isInvincible = default;



    private void Start()
    {
        // Loads the array with each child sprite's sprite renderer
        renderers = GetComponentsInChildren<SpriteRenderer>();

        // Sets the number of original colors to load
        originalColors = new Color[renderers.Length];

        // Loads the array with each child sprite's sprite renderer color
        for (int i = 0; i < originalColors.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }

        //// Loads the mask material for each child sprite and matches the matDefault with the original sprite renderer material
        //foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        //{
        //    matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        //    matDefault = new Material(Shader.Find("Sprites/Default"));
        //}
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
        if (isInvincible) return;

        // Decreased the health of the game object by the collinding object's damage amount
        health -= baseObjectController.GetDamage();

        // If the received damage reduces the game objects health to 0 or less it destroys it, otherwise it initiates a damage flash
        if (health <= 0)
        {
            Debug.Log("Dead");
            health = 0;
            Die();
            return;
        }
        if (damageFlash == true)
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
            Debug.Log("Player turned invincible!");
            isInvincible = true;

            for (float k = 0; k < numberOfFlashes * flashSpeed; k += flashSpeed)
            {
                foreach (SpriteRenderer r in renderers)
                {
                    // Sets each sprite in the renderers array to the color set in the collideColor variable
                    r.color = flashColor;
                }
                //foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
                //{
                //    // Sets the material in each child sprite's sprite renderer to white
                //    sr.material = matWhite;
                //}

                yield return new WaitForSeconds(flashSpeed);

                for (int i = 0; i < originalColors.Length; i++)
                {
                    // Returns each sprite in the renderers array to the color set in the originalColors variable
                    renderers[i].color = originalColors[i];
                }
                //foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
                //{
                //    // Returns all the child sprite's sprite renderer material back to what it was
                //    sr.material = matDefault;
                //}
                yield return new WaitForSeconds(flashSpeed);
            }

            Debug.Log("Player is no longer invincible!");
            isInvincible = false;
    }

    private void Die()
    {
        if (explosions != null)
            foreach (ExplosionController oneExpl in explosions)
                if (oneExpl != null)
                    oneExpl.StartExplosion();
    }
}
