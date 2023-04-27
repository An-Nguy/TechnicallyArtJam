using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

namespace Thuleanx.TArt {
    [RequireComponent(typeof(PlayerInput))]
    public partial class Player : MonoBehaviour {

#region Components
        public PlayerInput Input {get; private set; }
        public Animator Anim { get; private set; }
#endregion

        [SerializeField] float movementSpeed = 4;
        [SerializeField, ReadOnly] Vector3 velocity;

        void Awake() {
            Input = GetComponent<PlayerInput>();
            Anim = GetComponent<Animator>();
        }

        void Update() {
            velocity = Input.WorldSpace_Movement * movementSpeed;
            Move(velocity * Time.deltaTime);

            if (velocity.sqrMagnitude > 0)
                transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        void LateUpdate() {
            Anim.SetFloat("Speed", velocity.sqrMagnitude);
        }

        void Move(Vector3 displacement) {
            // we only execute code if moving, so no excessive navmesh queries
            if (displacement.sqrMagnitude> 0) {
                displacement = AdjustVelocityToSlope(displacement, 60);
                Vector3 nxtPos = displacement + transform.position;
                // prevent moving off navmesh
                if (FindClosestNavPoint(nxtPos, out Vector3 posOnNavMesh))
                    transform.position = posOnNavMesh;
            }
        }

        // Requires that the object's transform / pivot is at its base
		Vector3 AdjustVelocityToSlope(Vector3 velocity, float slopeLimitDegree) {
			float slideFriction = 0.3f;
			var ray = new Ray(transform.position + Vector3.down* 0.005f, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				Vector3 hitNormal = hit.normal;
				bool isGrounded = (Vector3.Angle (Vector3.up, hitNormal) <= slopeLimitDegree);

				if (!isGrounded) {
					velocity.x += (1f - hitNormal.y) * hitNormal.x * (1f - slideFriction);
					velocity.z += (1f - hitNormal.y) * hitNormal.z * (1f - slideFriction);
				} 

				velocity += Physics.gravity * Time.deltaTime;
				return velocity;
			}
			return velocity;
		}

		bool FindClosestNavPoint(Vector3 pos, out Vector3 resPos) {
			if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 0.5f, NavMesh.AllAreas)) {
				resPos = hit.position;
				return true;
			}
			resPos = pos;
			return false;
		}
    }
}
