using UnityEngine;

namespace Thuleanx.TArt {

[RequireComponent(typeof(Camera))]
public class WaterCamera : MonoBehaviour {
    Camera _waterCamera;

    [SerializeField] Material _waterMaterial;
    [SerializeField] string _waterViewMatrixParam;
    [SerializeField] string _waterProjectionMatrixParam;

    void Awake() {
        _waterCamera = GetComponent<Camera>();
    }

    void Update() {
        _waterMaterial.SetMatrix(_waterViewMatrixParam, _waterCamera.worldToCameraMatrix);
        _waterMaterial.SetMatrix(_waterProjectionMatrixParam, _waterCamera.projectionMatrix);
    }
}

}
