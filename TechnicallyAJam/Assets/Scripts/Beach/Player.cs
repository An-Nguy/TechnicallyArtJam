using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using NaughtyAttributes;
using Thuleanx.AI.FSM;

namespace Thuleanx.TArt {
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public partial class Player : StateMachine<Player> {

#region Components
        public PlayerInput Input {get; private set; }
        public Animator Anim { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
#endregion

        [BoxGroup("Movement")]
        [SerializeField] float movementSpeed = 4;
        [BoxGroup("Movement")]
        [SerializeField, ReadOnly] Vector3 velocity;

        [BoxGroup("Animation")]
		[SerializeField, ReorderableList] List<UnityEvent> animationEvents = new List<UnityEvent>();
        [BoxGroup("Animation")]
        [SerializeField, ReadOnly] bool waitingForTrigger = false;
		public bool WaitingForTrigger => waitingForTrigger;

        void Awake() {
            Input = GetComponent<PlayerInput>();
            Anim = GetComponent<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Start() {
            Construct();
        }

        void Update() {
            FindClosestNavPoint(transform.position, out Vector3 positionOnNavMesh);
            transform.position = positionOnNavMesh;
            NavMeshAgent.nextPosition = transform.position; // sync navmesh agent position with current
            this.RunUpdate();
            NavMeshAgent.velocity = velocity;
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

		void StartAnimationWait() => waitingForTrigger = true;
        public void AnimationTrigger() => waitingForTrigger = false;

		public void _TriggerAnimatedEvent(AnimationEvent animEvent) {
			if (animEvent.animatorClipInfo.weight > 0.8f) animationEvents[animEvent.intParameter]?.Invoke();
		}

		public void _TriggerAnimatedEventUnweighted(int eventIndex) => animationEvents[eventIndex]?.Invoke();

        IEnumerator _waitForTrigger() {
            StartAnimationWait();
            while (waitingForTrigger) yield return null;
        }

        public override void Construct() {
            ConstructMachine(this, Enum.GetNames(typeof(Player.State)).Length, (int) Player.State.Grounded, false);

            AssignState((int) Player.State.Grounded, new PlayerGrounded());
            AssignState((int) Player.State.Pickup, new PlayerPickup());
        }

    }
}
