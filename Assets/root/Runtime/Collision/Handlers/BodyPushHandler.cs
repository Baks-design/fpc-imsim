using System;
using Assets.root.Runtime.Collision.Interfaces;
using Assets.root.Runtime.Collision.Settings;
using Assets.root.Runtime.Utilities.Extensions;
using UnityEngine;

namespace Assets.root.Runtime.Collision.Handlers
{
	public class BodyPushHandler : IBodyPushHandler
	{
		readonly CollisionBodyPushSettings bodyPushSettings;
		static readonly Vector3 HorizontalMask = new(1f, 0f, 1f);
		const float VerticalPushThreshold = -0.3f;

        public BodyPushHandler(CollisionBodyPushSettings bodyPushSettings)
		=> this.bodyPushSettings = bodyPushSettings != null ? bodyPushSettings :
			throw new ArgumentNullException(nameof(bodyPushSettings));

        public void PushRigidBodiesHandler(ControllerColliderHit hit)
		{
			if (bodyPushSettings.canPush)
				PushRigidBodies(hit);
		}

		void PushRigidBodies(ControllerColliderHit hit)
		{
			if (hit.collider.attachedRigidbody == null ||
				hit.collider.attachedRigidbody.isKinematic ||
				hit.moveDirection.y < VerticalPushThreshold) return;

			if (!bodyPushSettings.pushLayers.Contains(hit.collider.gameObject.layer)) return;

			var pushDir = Vector3.Scale(hit.moveDirection, HorizontalMask);

			hit.collider.attachedRigidbody.AddForce(pushDir * bodyPushSettings.strength, ForceMode.Impulse);
		}
	}
}