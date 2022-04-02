using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
    public SphereCollider collider;
    public float gravityDistance, gravityPower;
    public bool pull = true;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public float GetGravityPower() {
        return gravityPower;
    }
    public int GetGravityDirection() {
        return pull ? 1 : -1;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Player")
            return;

        other.GetComponent<Player>().AddGravity(this);
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag != "Player")
            return;

        other.GetComponent<Player>().RemoveGravity(this);
    }
    [ExecuteInEditMode]
    void OnValidate() {
        if (collider != null)
            collider.radius = gravityDistance;
    }
}
