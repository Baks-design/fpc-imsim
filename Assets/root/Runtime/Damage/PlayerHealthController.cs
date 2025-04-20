using KBCore.Refs;
using UnityEngine;

namespace Assets.root.Runtime.Damage
{
    public class PlayerHealthController : Health
    {
        [SerializeField, Parent] CharacterController character;
        [SerializeField] float KillHeight = -50f;

        void OnEnable()
        {
            OnDie += Death;
            OnFall += FallDamage;
        }

        void OnDisable()
        {
            OnDie -= Death;
            OnFall -= FallDamage;
        }

        void Death() => IsDead = true;

        void FallDamage(float dmgFromFall) => TakeDamage(dmgFromFall, null);

        protected override void Start() => base.Start();

        void Update() => CheckYForKill();

        void CheckYForKill()
        {
            if (!IsDead && character.transform.position.y < KillHeight)
                Kill();
        }
    }
}