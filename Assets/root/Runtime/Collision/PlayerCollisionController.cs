using UnityEngine;
using KBCore.Refs;
using Assets.root.Runtime.Movement;
using Assets.root.Runtime.Collision.Settings;
using Assets.root.Runtime.Collision.Interfaces;
using Assets.root.Runtime.Collision.Handlers;

namespace Assets.root.Runtime.Collision
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Parent] CharacterController character;
        [SerializeField, Anywhere] PlayerMovementController movement;
        [Header("Settings")]
        [SerializeField] CollisionBodyPushSettings bodyPushSettings;
        [SerializeField] GroundCheckSettings groundSettings;
        IBodyPushHandler BodyPushHandler;

        public IGroundChecker GroundChecker { get; private set; }
        public ICeilChecker CeilChecker { get; private set; }
        public IObstacleChecker ObstacleChecker { get; private set; }

        void Awake()
        {
            GroundChecker = new GroundCollisionHandler(groundSettings, character);
            ObstacleChecker = new ObstacleCollisionHandler(character);
            CeilChecker = new CeilCollisionHandler(character);
            BodyPushHandler = new BodyPushHandler(bodyPushSettings);
        }

        void Update()
        {
            GroundChecker.UpdateGroundCheck();
            CeilChecker.UpdateCeilCheck();
            ObstacleChecker.UpdateObstacleCheck(movement.MovementHandler);
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        => BodyPushHandler.PushRigidBodiesHandler(hit);
    }
}