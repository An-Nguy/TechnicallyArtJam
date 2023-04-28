using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using Thuleanx.Utils;

namespace Thuleanx.TArt {
    [RequireComponent(typeof(Thuleanx.TArt.PlayerInput))]
    public class PlayerInputSystem : MonoBehaviour {

#region Components
        public PlayerInput PlayerInput {get; private set; }
#endregion

        public bool overriden = false;

        [SerializeField, ReadOnly] Vector2 movement;
        [SerializeField] Timer interact;

        void Awake() {
            PlayerInput = GetComponent<PlayerInput>();
            interact = 0.2f;
        }

        void OnEnable() => PlayerInput.OnInteract.AddListener(OnInteract);
        void OnDisable() => PlayerInput.OnInteract.RemoveListener(OnInteract);
        void OnInteract() => interact.Stop();

        void Update() {
            if (!overriden) {
                PlayerInput.Movement = movement;
                PlayerInput.Interact = interact;
            }
        }

        public void OnMovement(InputAction.CallbackContext ctx) => movement = ctx.ReadValue<Vector2>();
        public void OnInteract(InputAction.CallbackContext ctx) {
            if (ctx.started) interact.Start();
        }
    }
}
