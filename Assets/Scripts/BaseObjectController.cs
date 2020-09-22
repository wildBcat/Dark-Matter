using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectController : MonoBehaviour
{
    [Tooltip("Sets the amount of damage game object deals on collision with another object. " +
        "The higher the number, the more damage it assigns during each collision.")]
    [SerializeField] int damage = default;

    [Tooltip("Sets The amount of health game object has. Any damage assigned to the game object will reduce this number. " +
        "The higher the number, the more it will take to kill the game object.")]
    [SerializeField] int health = default;

    [Tooltip("Sets the color the sprite will flash when damage is assigned. " +
        "If this isn't set then the mat will take on the color of the sprite.")]
    [SerializeField] Color collideColor = default;

    private Material matWhite;    
    private Material matDefault;    
    SpriteRenderer[] renderers;
    Color[] originalColors;

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

        // Loads the mask material for each child sprite and matches the matDefault with the original sprite renderer material
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
            matDefault = sr.GetComponent<SpriteRenderer>().material;
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
        // Decreased the health of the game object by the collinding object's damage amount
        health -= baseObjectController.GetDamage();

        // If the received damage reduces the game objects health to 0 or less it destroys it, otherwise it initiates a damage flash
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
        for (int j = 0; j < 3; j++)
        {
            foreach (SpriteRenderer r in renderers)
            {
                // Sets each sprite in the renderers array to the color set in the collideColor variable
                r.color = collideColor;
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                // Sets the material in each child sprite's sprite renderer to white
                sr.material = matWhite;
            }

            yield return new WaitForSeconds(.1f);
            
            for (int i = 0; i < originalColors.Length; i++)
            {
                // Returns each sprite in the renderers array to the color set in the originalColors variable
                renderers[i].color = originalColors[i];
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                // Returns all the child sprite's sprite renderer material back to what it was
                sr.material = matDefault;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private void Die()
    {
        // Assigns the allExplosions variable to all the objects with an expolosion controller script
        ExplosionController[] allExplosions = GameObject.FindObjectsOfType<ExplosionController>();

        // Triggers the explosion for each one stored in allExplosions
        foreach (ExplosionController oneExpl in allExplosions)
            oneExpl.StartExplosion();
    }


}
