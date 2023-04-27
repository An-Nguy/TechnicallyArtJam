using UnityEngine;
using UnityEngine.InputSystem;

namespace Thuleanx.TArt {
    [RequireComponent(typeof(Thuleanx.TArt.PlayerInput))]
    public class PlayerInputSystem : MonoBehaviour {

#region Components
        public PlayerInput PlayerInput {get; private set; }
#endregion

        void Awake() {
            PlayerInput = GetComponent<PlayerInput>();
        }

        public void OnMovement(InputAction.CallbackContext ctx) {
            PlayerInput.Movement = ctx.ReadValue<Vector2>();
        }
    }
}
