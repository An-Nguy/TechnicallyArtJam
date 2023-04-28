using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using Thuleanx.Utils;

namespace Thuleanx.TArt {
    public class PlayerInput : MonoBehaviour {
        [ReadOnly] public Vector2 Movement = Vector2.zero;
        [ReadOnly] public bool Interact = false;

        // input movement as a world direction
        // get this by rotating input movement by camera's y rotation
        public Vector3 WorldSpace_Movement 
            => Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) *
                (new Vector3(Movement.x, 0, Movement.y)).normalized; 

        public void SetMovement_WorldSpace(Vector3 ws_dir) {
            Vector3 inputSpace_dir = 
                Quaternion.Euler(0, -Camera.main.transform.eulerAngles.y, 0) * ws_dir;

            Movement = new Vector2(inputSpace_dir.x, inputSpace_dir.z).normalized;
        }

        [HideInInspector] public UnityEvent OnInteract;
    }
}
