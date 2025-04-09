using System;
using UnityEngine;

namespace Assets.root.Runtime.Movement.Handlers
{
	public class BodyPushHandler : IBodyPushHandler
	{
		readonly BodyPushSettings bodyPushSettings;
		static readonly Vector3 HorizontalMask = new(1f, 0f, 1f);
		const float VerticalPushThreshold = -0.3f;

        public BodyPushHandler(BodyPushSettings bodyPushSettings)
		=> this.bodyPushSettings = bodyPushSettings ?? throw new ArgumentNullException(nameof(bodyPushSettings));

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