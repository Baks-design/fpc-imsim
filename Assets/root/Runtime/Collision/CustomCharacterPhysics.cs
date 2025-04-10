using UnityEngine;

namespace Assets.root.Runtime.Movement
{
    public static class CustomCharacterPhysics 
    {
        #region Static Cache System
        static class PhysicsCache
        {
            public static readonly RaycastHit[] GroundHits = new RaycastHit[1];
            public static readonly RaycastHit[] StepHits = new RaycastHit[1];
            public static readonly RaycastHit[] RaycastHits = new RaycastHit[4];
            public static readonly Collider[] SmallColliderCache = new Collider[8];
            public static readonly Collider[] MediumColliderCache = new Collider[16];
            public static readonly Collider[] LargeColliderCache = new Collider[32];
            public static readonly RaycastHit[] SphereCastHits = new RaycastHit[4];
        }
        #endregion

        #region Non-Allocating Ground Detection
        /// <summary>
        /// Non-allocating ground check with detailed hit information
        /// </summary>
        public static bool IsCharacterGrounded(
            CharacterController controller, out RaycastHit groundHit, float groundCheckOffset = 0.1f,
            float groundCheckDistance = 0.2f, LayerMask groundLayer = default)
        {
            groundHit = new RaycastHit();

            if (groundLayer.value == 0)
                groundLayer = Physics.DefaultRaycastLayers;

            var rayStart = controller.transform.position + Vector3.up * (controller.skinWidth + groundCheckOffset);
            var rayLength = controller.skinWidth + groundCheckDistance;

            var hits = Physics.SphereCastNonAlloc(
                rayStart, controller.radius, Vector3.down, PhysicsCache.GroundHits, rayLength, groundLayer);

            // Debug visualization
            Debug.DrawLine(rayStart, rayStart + Vector3.down * rayLength, hits > 0 ? Color.green : Color.red, 0.1f);

            if (hits > 0)
            {
                groundHit = PhysicsCache.GroundHits[0];
                return true;
            }

            return controller.isGrounded;
        }

        /// <summary>
        /// Gets ground normal without allocation
        /// </summary>
        public static bool GetGroundNormal(
            CharacterController controller, out Vector3 groundNormal, float groundCheckOffset = 0.1f,
            float groundCheckDistance = 0.2f, LayerMask groundLayer = default)
        {
            groundNormal = Vector3.up;

            if (IsCharacterGrounded(controller, out var groundHit, groundCheckOffset, groundCheckDistance, groundLayer))
            {
                groundNormal = groundHit.normal;
                return true;
            }

            return false;
        }
        #endregion

        #region Non-Allocating Collision Detection
        /// <summary>
        /// Gets all touching colliders without allocation
        /// </summary>
        public static int GetTouchingColliders(
            CharacterController controller, Collider[] results,
            QueryTriggerInteraction queryMode = QueryTriggerInteraction.Ignore)
        {
            if (results == null || results.Length == 0) return 0;

            var bottom = controller.transform.position + Vector3.up * controller.radius;
            var top = controller.transform.position + Vector3.up * (controller.height - controller.radius);

            return Physics.OverlapCapsuleNonAlloc(
                bottom, top, controller.radius * 1.1f, results, Physics.DefaultRaycastLayers, queryMode);
        }

        /// <summary>
        /// Checks for overhead obstacles without allocation
        /// </summary>
        public static bool CheckHeadroom(
            CharacterController controller, out RaycastHit overheadHit, float minHeadroom = 0.1f, float radius = 0.9f)
        {
            overheadHit = new RaycastHit();

            var rayStart = controller.transform.position + Vector3.up * (controller.height - controller.radius);
            var rayLength = minHeadroom;

            var hits = Physics.SphereCastNonAlloc(
                rayStart, controller.radius * radius, Vector3.up, PhysicsCache.RaycastHits, rayLength,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            if (hits > 0)
            {
                overheadHit = PhysicsCache.RaycastHits[0];
                return true;
            }

            return false;
        }
        #endregion

        #region Non-Allocating Movement Physics
        /// <summary>
        /// Non-allocating movement check with obstacle detection
        /// </summary>
        public static bool CheckMovement(
            CharacterController controller, Vector3 direction,
            out RaycastHit obstacleHit, float distance = 0.5f, float padding = 0.1f)
        {
            obstacleHit = new RaycastHit();

            var origin = controller.transform.position + Vector3.up * (controller.height * 0.5f);

            var hits = Physics.SphereCastNonAlloc(
                origin, controller.radius - padding, direction, PhysicsCache.RaycastHits, distance,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            if (hits > 0)
            {
                obstacleHit = PhysicsCache.RaycastHits[0];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Non-allocating step detection and handling
        /// </summary>
        public static bool TryStepUp(
            CharacterController controller, ref Vector3 motion,
            out RaycastHit stepHit, float stepHeight = 0.3f, float stepCheckDistance = 0.1f)
        {
            stepHit = new RaycastHit();

            if (controller.isGrounded && motion.magnitude > 0.01f)
            {
                var moveDirection = motion.normalized;
                var rayOrigin = controller.transform.position + Vector3.up * (controller.skinWidth + 0.01f);

                var hits = Physics.RaycastNonAlloc(
                    rayOrigin, moveDirection, PhysicsCache.StepHits, controller.radius + stepCheckDistance,
                    Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

                if (hits > 0)
                {
                    stepHit = PhysicsCache.StepHits[0];
                    var stepPos = stepHit.point + moveDirection * controller.radius;
                    stepPos.y = controller.transform.position.y + stepHeight;

                    if (Physics.CheckSphere(
                        stepPos, controller.radius * 0.9f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) == false)
                    {
                        float stepUpAmount = stepPos.y - controller.transform.position.y;
                        motion.y += stepUpAmount / Time.deltaTime;
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        #region Non-Allocating Environmental Queries
        /// <summary>
        /// Finds colliders in a sphere around the character without allocation
        /// </summary>
        public static int OverlapSphereAroundCharacter(
            CharacterController controller, float radius, Collider[] results, LayerMask layerMask = default,
            QueryTriggerInteraction queryMode = QueryTriggerInteraction.UseGlobal)
        {
            if (layerMask.value == 0)
                layerMask = Physics.DefaultRaycastLayers;

            return Physics.OverlapSphereNonAlloc(controller.transform.position, radius, results, layerMask, queryMode);
        }

        /// <summary>
        /// Checks for nearby ledges without allocation
        /// </summary>
        public static bool CheckLedgeAhead(
            CharacterController controller, Vector3 moveDirection, out RaycastHit ledgeHit,
            float checkDistance = 1f, float stepHeight = 0.5f)
        {
            ledgeHit = new RaycastHit();

            var origin = controller.transform.position + Vector3.up * (controller.skinWidth + stepHeight);

            // First check if there's ground in front of us
            if (Physics.RaycastNonAlloc(
                origin, moveDirection, PhysicsCache.RaycastHits, checkDistance, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore) == 0)
            {
                // Now check downward to see if there's a drop
                var edgeCheckPos = origin + moveDirection * checkDistance;

                if (Physics.RaycastNonAlloc(
                    edgeCheckPos, Vector3.down, PhysicsCache.RaycastHits, stepHeight * 2f,
                    Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) == 0)
                {
                    ledgeHit.point = edgeCheckPos;
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Physics-Based Movement Calculations
        /// <summary>
        /// Calculates movement velocity with acceleration/deceleration
        /// </summary>
        public static Vector3 CalculateVelocity(
            Vector3 currentVelocity, Vector3 desiredDirection, float acceleration,
            float maxSpeed, bool isGrounded, float gravity, float deltaTime)
        {
            // Calculate target velocity
            var targetVelocity = desiredDirection * maxSpeed;

            // Apply acceleration to horizontal movement
            var velocityChange = targetVelocity - currentVelocity;
            velocityChange.y = 0f; // Only affect horizontal movement

            // Clamp the velocity change by acceleration
            var maxChange = acceleration * deltaTime;
            if (velocityChange.magnitude > maxChange)
                velocityChange = velocityChange.normalized * maxChange;

            // Apply the velocity change
            var newVelocity = currentVelocity + velocityChange;

            // Apply gravity if not grounded
            if (!isGrounded)
                newVelocity.y += gravity * deltaTime;
            else if (newVelocity.y < 0f)
                newVelocity.y = 0f; // Reset vertical velocity when grounded

            return newVelocity;
        }

        /// <summary>
        /// Applies friction to movement (ground only)
        /// </summary>
        public static Vector3 ApplyFriction(
            Vector3 currentVelocity, float frictionAmount, bool isGrounded, float deltaTime)
        {
            if (isGrounded)
            {
                var reduction = frictionAmount * deltaTime;
                var horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

                if (horizontalVelocity.magnitude > reduction)
                    horizontalVelocity -= horizontalVelocity.normalized * reduction;
                else
                    horizontalVelocity = Vector3.zero;

                return new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
            }

            return currentVelocity;
        }

        /// <summary>
        /// Applies a jump force to the velocity
        /// </summary>
        public static Vector3 ApplyJump(Vector3 currentVelocity, float jumpForce, float gravity)
        {
            currentVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            return currentVelocity;
        }

        /// <summary>
        /// Calculates air control movement
        /// </summary>
        public static Vector3 CalculateAirControl(
            Vector3 currentVelocity, Vector3 desiredDirection, float airControl, float maxAirSpeed, float deltaTime)
        {
            var horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            var targetVelocity = desiredDirection * maxAirSpeed;

            // Only apply air control if trying to move
            if (desiredDirection.magnitude > 0.1f)
            {
                var velocityChange = targetVelocity - horizontalVelocity;
                var maxChange = airControl * deltaTime;

                if (velocityChange.magnitude > maxChange)
                    velocityChange = velocityChange.normalized * maxChange;

                horizontalVelocity += velocityChange;

                // Clamp to max air speed
                if (horizontalVelocity.magnitude > maxAirSpeed)
                    horizontalVelocity = horizontalVelocity.normalized * maxAirSpeed;
            }

            return new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
        }
        #endregion

        #region Slope Handling
        /// <summary>
        /// Checks if the slope is too steep to climb
        /// </summary>
        public static bool IsSlopeTooSteep(
            CharacterController controller, float maxSlopeAngle = 45f, float groundCheckOffset = 0.1f,
            float groundCheckDistance = 0.2f, LayerMask groundLayer = default)
        {
            if (GetGroundNormal(controller, out var groundNormal, groundCheckOffset, groundCheckDistance, groundLayer))
            {
                var slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
                return slopeAngle > maxSlopeAngle;
            }

            return false;
        }

        /// <summary>
        /// Adjusts movement direction based on slope normal
        /// </summary>
        public static Vector3 AdjustMovementForSlope(Vector3 movement, Vector3 groundNormal, float maxSlopeAngle = 45f)
        {
            var slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
            if (slopeAngle <= maxSlopeAngle)
                // Project movement direction onto the slope plane
                return Vector3.ProjectOnPlane(movement, groundNormal).normalized * movement.magnitude;

            return movement;
        }
        #endregion
    }
}