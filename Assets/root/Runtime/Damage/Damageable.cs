using KBCore.Refs;
using UnityEngine;

namespace Assets.root.Runtime.Damage
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField, Self] Health health;
        [Tooltip("Multiplier to apply to the received damage")]
        public float DamageMultiplier = 1f;
        [Tooltip("Multiplier to apply to self damage")]
        [Range(0f, 1f)] public float SensibilityToSelfdamage = 0.5f;

        public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
        {
            if (health)
            {
                var totalDamage = damage;

                // skip the crit multiplier if it's from an explosion
                if (!isExplosionDamage)
                    totalDamage *= DamageMultiplier;

                // potentially reduce damages if inflicted by self
                if (health.gameObject == damageSource)
                    totalDamage *= SensibilityToSelfdamage;

                // apply the damages
                health.TakeDamage(totalDamage, damageSource);
            }
        }
    }
}