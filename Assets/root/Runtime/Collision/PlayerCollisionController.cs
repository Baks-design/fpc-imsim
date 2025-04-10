using UnityEngine;
using KBCore.Refs;
using Assets.root.Runtime.Movement.Handlers;
using Assets.root.Runtime.Movement.Interfaces;
using Assets.root.Runtime.Movement.Settings;

namespace Assets.root.Runtime.Movement
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Parent] CharacterController character;
        [Header("Settings")]
        [SerializeField] CollisionBodyPushSettings bodyPushSettings;
        [SerializeField] MovementJumpSettings jumpSettings;

        public IGroundChecker GroundChecker { get; private set; }
        public ICeilChecker CeilChecker { get; private set; }
        public IWallChecker WallChecker { get; private set; }
        public IBodyPushHandler BodyPushHandler { get; private set; }

        void Awake()
        {
            GroundChecker = new GroundCollisionHandler(character, jumpSettings);
            WallChecker = new WallCollisionHandler(character);
            CeilChecker = new CeilCollisionHandler(character);
            BodyPushHandler = new BodyPushHandler(bodyPushSettings);
        }

        void Update()
        {
            GroundChecker.UpdateGroundCheck();
            CeilChecker.UpdateCeilCheck();
            WallChecker.UpdateWallCheck();
        }

        void OnControllerColliderHit(ControllerColliderHit hit) => BodyPushHandler.PushRigidBodiesHandler(hit);

        void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            GroundChecker.DrawDebugGizmos();
            CeilChecker.DrawDebugGizmos();
            WallChecker.DrawDebugGizmos();
        }
    }
}