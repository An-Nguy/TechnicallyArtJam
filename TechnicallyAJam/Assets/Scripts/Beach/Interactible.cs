using UnityEngine;

namespace Thuleanx.TArt {

public class Interactible : MonoBehaviour {
    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") other.GetComponentInParent<Player>().Interactibles.Add(this);
    }

    public void OnTriggerExit(Collider other) {
        if (other.tag == "Player") other.GetComponentInParent<Player>().Interactibles.Remove(this);
    }
}

}
