﻿using UnityEngine;
using System.Collections;
using System.Linq;

namespace ND_VariaBULLET
{
    public class GameObjectLifeController : ShotCollision, IShotCollidable
    {
        [Header("Health Settings")]
        [Tooltip("Sets the amount of live the gameobject has. Reduces according to incoming IDamager.DMG value upon collision.")]
        public float healthPoints = default;

        [Header("Damage Settings")]
        [Tooltip("Sets whether or not you want the gameobject to flash a color when damaged. Checked is yes, unchecked is no.")]
        public bool DamageFlicker;
        [Tooltip("Sets the number of frames the Damage Flicker will last once triggered.")]
        public int FlickerDuration = 6;
        [Tooltip("Sets the color the sprite will flash when damage is assigned.")]
        [SerializeField] Color flickerColor = default;

        [Header("Death Settings")]
        [Tooltip("Sets which explosions to trigger when the gameobject's life is reduced to <= 0.")]
        public ExplosionController[] explosions;

        private SpriteRenderer[] renderers = default;
        private Color[] originalColors = default;

        private bool isInvincible = default;

        /*
         * Loads the array with each child sprite's sprite renderer, but only if they are tagged "Parts" 
         * Sets the number of original colors to load
         * Loads the array with each child sprite's sprite renderer color
         */
        private void Start()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
            renderers = renderers.Where(child => child.CompareTag("Parts")).ToArray();
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < originalColors.Length; i++)
            {
                originalColors[i] = renderers[i].color;
            }
        }

        /* 
         * When a laser collides with this game object, if the laser's collision layer is in this game object's collision list, do the following:
         * 1. Check if it is invincible (if it is, don't do anything).
         * 2. If not invincible, Instantiate the listed explosion for this type of collision
         * 3. If not invincible, begin the damage coroutine
         */
        public new IEnumerator OnLaserCollision(CollisionArgs sender)
        {
            if (CollisionFilter.CollisionAccepted(sender.gameObject.layer, CollisionList))
            {
                if (isInvincible)
                {
                    yield break;
                }
                else
                {
                    CollisionFilter.SetExplosion(LaserExplosion, ParentExplosion, this.transform, new Vector2(sender.point.x, sender.point.y), 0, this);
                    StartCoroutine(SetDamage(sender.damage));
                }
            }
        }

        /* 
        * When a laser collides with this game object, if the shot's collision layer is in this game object's collision list, do the following:
        * 1. Check if it is invincible (if it is, don't do anything).
        * 2. If not invincible, Instantiate the listed explosion for this type of collision
        * 3. If not invincible, begin the damage coroutine
        */
        public new IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionFilter.CollisionAccepted(collision.gameObject.layer, CollisionList))
            {
                if (isInvincible)
                {
                    yield break;
                }
                else
                {
                    CollisionFilter.SetExplosion(BulletExplosion, ParentExplosion, this.transform, collision.contacts[0].point, 0, this);
                    StartCoroutine(SetDamage(collision.gameObject.GetComponent<IDamager>().DMG));
                }
            }
        }

        /* Reduces the amount of health of this game object by the amount of damage assigned in the colliding object
         * If the health of this game object drops to 0 or below, Initiate any explosions assigned to the game object
         * If the health of this game object does not drop to 0 or below, Initiate damage flicker
         */
        protected IEnumerator SetDamage(float damage)
        {
            healthPoints -= damage;
            if (healthPoints <= 0)
            {
                // Initiates all the explosions attached to the object
                if (explosions != null)
                    foreach (ExplosionController oneExpl in explosions)
                        if (oneExpl != null)
                            oneExpl.StartExplosion();
                yield break;
            }
            yield return SetFlicker();
        }

        /*
         * If damage flicker is enabled, do the following:
         * 1. Set bool value "incincible" to true
         * 2. Set bool value "flicker" to false
         * 3. For the duration of the flicker length set, do the following:
         *  A. Set bool value flicker to no false
         *  b. If flicker is true, set all the sprite render colors in "renders" to the flicker color
         *  c. Otherwise, set them all to their original color
         * 4. set all the sprite render colors in "renders" to their original color
         * 5. Set bool value "incincible" to false
         * If damage flicker in not enabled, do nothing
         */
        protected IEnumerator SetFlicker()
        {
            if (DamageFlicker)
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
            else
            {
                yield break;
            }
        }
    }
}