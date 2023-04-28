using UnityEngine;
using NaughtyAttributes;

namespace Thuleanx.TArt {
    public class PlayerInput : MonoBehaviour {
        [ReadOnly]
        public Vector2 Movement = Vector2.zero;

        // input movement as a world direction
        // get this by rotating input movement by camera's y rotation
        public Vector3 WorldSpace_Movement 
            => Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) *
                (new Vector3(Movement.x, 0, Movement.y)).normalized; 
    }
}
