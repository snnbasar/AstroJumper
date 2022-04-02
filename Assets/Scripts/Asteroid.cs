using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Asteroid : MonoBehaviour {
    public GameObject targetAsteroid;
    public float eulerAngleSpeedPerSec = 60;
    public static readonly float CATCH_DISTANCE = 2.8f;
    void Awake() {
        transform.LookAt(targetAsteroid.transform, transform.up);
    }
    void Start() {
        //StartRotation();
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(new Vector3(0, eulerAngleSpeedPerSec * Time.deltaTime, 0));
    }
    public Vector3 GetCamPosition() {
        Vector3 x = GetNormalizedTargetDirection(),
            y = transform.up;

        return (12f * x) + (8f * y) + transform.position;
    }
    public Vector3 GetCamNormalizedDirection(Vector3 camPosition) {
        return (targetAsteroid.transform.position - camPosition).normalized;
    }
    public Vector3 GetNormalizedTargetDirection() {
        return (transform.position - targetAsteroid.transform.position).normalized;
    }
    public void Grap(Player player) {
        //GetComponent<SphereCollider>().enabled = false;
        //targetAsteroid.GetComponent<SphereCollider>().enabled = true;
        //Destroy(GetComponent<SphereCollider>());

        // Camera Move
        if (player is TutorialPlayer) {
            ((TutorialPlayer)player).Grap(gameObject);
            return;
        }
        Vector3 camPosition = GetCamPosition();
        Camera.main.GetComponent<Cam>().MoveTo(camPosition, GetCamNormalizedDirection(camPosition));

        player.Grap(gameObject);
    }
    public float GetTargetDistance() {
        return Vector3.Distance(targetAsteroid.transform.position, transform.position);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Player")
            return;

        Player player = other.GetComponent<Player>();

        // Doğru asteroid mi
        if (player.GetTarget().Equals(gameObject))
            Grap(player);
    }
    public void TestCam() {
        Vector3 camPosition = GetCamPosition();
        Camera.main.GetComponent<Cam>().MoveTo(camPosition, GetCamNormalizedDirection(camPosition));

    }
}