using UnityEngine;
using System.Collections;
using System.Linq;

namespace ND_VariaBULLET
{
    public class BaseObjectController1 : ShotCollision, IShotCollidable
    {
        [Tooltip("Sets the number of frames the Damage Flicker will last once triggered.")]
        public int FlickerDuration = 6;
        [Tooltip("Sets the color the sprite will flash when damage is assigned. " +
            "If this isn't set then the mat will take on the color of the sprite.")]
        [SerializeField] Color flickerColor = default;
        [Tooltip("Health Points. Reduces according to incoming IDamager.DMG value upon collision.")]
        public float HP = default;
        [Tooltip("Array for all explosions for this ship")]
        public ExplosionController[] explosions;
        private SpriteRenderer[] renderers = default;
        Color[] originalColors = default;
        private bool isInvincible = default;

        private void Start()
        {
            // Loads the array with each child sprite's sprite renderer
            renderers = GetComponentsInChildren<SpriteRenderer>();
            renderers = renderers.Where(child => child.CompareTag("Parts")).ToArray();

            // Sets the number of original colors to load
            originalColors = new Color[renderers.Length];

            // Loads the array with each child sprite's sprite renderer color
            for (int i = 0; i < originalColors.Length; i++)
            {
                originalColors[i] = renderers[i].color;
            }
        }

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

        protected IEnumerator SetDamage(float damage)
        {
            HP -= damage;
            if (HP <= 0)
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

        protected IEnumerator SetFlicker()
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
    }
}