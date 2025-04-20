using System;
using UnityEngine;

namespace Assets.root.Runtime.Damage
{
    public abstract class Health : MonoBehaviour
    {
        [Tooltip("Maximum amount of health")]
        protected float MaxHealth = 10f;
        [Tooltip("Health ratio at which the critical health vignette starts appearing")]
        protected float CriticalHealthRatio = 0.3f;
        bool m_IsDead;

        public bool CanPickup => CurrentHealth < MaxHealth;
        public bool IsCritical => GetRatio <= CriticalHealthRatio;
        public float GetRatio => CurrentHealth / MaxHealth;
        public float CurrentHealth { get; set; }
        public bool Invincible { get; set; }
        public bool IsDead { get; set; }

        public Action<float, GameObject> OnDamaged = delegate { };
        public Action<float> OnHealed = delegate { };
        public Action OnDie = delegate { };
        public Action<float> OnFall = delegate { };

        protected virtual void Start() => CurrentHealth = MaxHealth;

        public void Heal(float healAmount)
        {
            var healthBefore = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            // call OnHeal action
            var trueHealAmount = CurrentHealth - healthBefore;
            if (trueHealAmount > 0f)
                OnHealed.Invoke(trueHealAmount);
        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (Invincible) return;

            var healthBefore = CurrentHealth;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            // call OnDamage action
            var trueDamageAmount = healthBefore - CurrentHealth;
            if (trueDamageAmount > 0f)
                OnDamaged.Invoke(trueDamageAmount, damageSource);

            HandleDeath();
        }

        public void Kill()
        {
            CurrentHealth = 0f;
            // call OnDamage action
            OnDamaged.Invoke(MaxHealth, null);
            HandleDeath();
        }

        void HandleDeath()
        {
            if (m_IsDead) return;

            // call OnDie action
            if (CurrentHealth <= 0f)
            {
                m_IsDead = true;
                OnDie.Invoke();
            }
        }
    }
}