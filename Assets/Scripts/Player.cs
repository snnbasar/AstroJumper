using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    protected Asteroid asteroid;
    protected Coroutine flyingCo, SendRayCastCo;
    protected Rigidbody rb;
    protected Animator anim;
    protected List<Gravity> gravityList;
    protected float currentO2, savedO2;
    protected bool isGrapping = false;
    public static readonly float ANIMATION_SPEED = 50f, O2_LIMIT = 100f, O2_PER_SEC = 2f, O2_SMALL = 20f, O2_BIG = 40f;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        gravityList = new List<Gravity>();
        currentO2 = O2_LIMIT;
    }

    // Update is called once per frame
    void Update() {
        if (gravityList.Count == 0 || isGrapping)
            return;

        for (int i = 0; i < gravityList.Count; i++) {
            Vector3 direction = gravityList[i].GetGravityDirection() * (gravityList[i].transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, gravityList[i].transform.position);
            float force = gravityList[i].GetGravityPower() / Mathf.Pow(distance, 2);

            rb.AddForce(direction * force);
        }
    }
    public GameObject GetTarget() {
        return asteroid.targetAsteroid;
    }
    public Asteroid GetCurrentAsteroid() {
        return asteroid;
    }
    public float GetO2() {
        return currentO2;
    }
    public void AddO2(float val) {
        currentO2 += val;

        if (currentO2 > O2_LIMIT)
            currentO2 = O2_LIMIT;
    }
    public bool ReduceO2(float val) {
        if(!Utility.isGameOver)
            currentO2 -= val;

        return currentO2 >= 0;
    }
    public void AddGravity(Gravity gravity) {
        if (asteroid.transform.Equals(gravity.transform.parent))
            return;

        gravityList.Add(gravity);
    }
    public void RemoveGravity(Gravity gravity) {
        gravityList.Remove(gravity);
    }
    public void Throw(float throwSpeed) {
        if (SendRayCastCo != null) 
            StopCoroutine(SendRayCastCo);

        isGrapping = false;
        Utility.isInputActive = false;

        transform.parent = null;
        rb.AddForce(-transform.forward * throwSpeed, ForceMode.Impulse);
        anim.SetBool("Zipla", true);
        savedO2 = currentO2;

        flyingCo = StartCoroutine(Flying());
    }
    public void Grap(GameObject __asteroid) {
        if (flyingCo != null)
            StopCoroutine(flyingCo);

        isGrapping = true;

        // Revived
        if (__asteroid.GetComponent<Asteroid>().Equals(asteroid))
            currentO2 = savedO2;

        // Remove current asteroid gravity
        asteroid = __asteroid.GetComponent<Asteroid>();
        gravityList.Remove(asteroid.GetComponentInChildren<Gravity>());

        // Stop
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Set Position
        //Vector3 normalizedDirection = (asteroid.transform.position - transform.position).normalized;
        //transform.position = asteroid.transform.position - normalizedDirection * Asteroid.CATCH_DISTANCE;
        //transform.LookAt(asteroid.transform, transform.up);

        // Set Position
        transform.position = asteroid.transform.position + asteroid.GetNormalizedTargetDirection() * Asteroid.CATCH_DISTANCE;
        transform.LookAt(asteroid.transform, asteroid.transform.up);
        //Debug.Log(transform.position);

        transform.SetParent(asteroid.transform);
        anim.SetBool("Zipla", false);

        //SendRayCastCo = StartCoroutine(SendRayCast());
    }
    public void Choked() {
        transform.parent = null;
        rb.AddForce(-transform.forward * 2f, ForceMode.Impulse);
    }
    IEnumerator Flying() {
        Transform target = asteroid.targetAsteroid.transform;
        Gravity targetGravity = asteroid.targetAsteroid.GetComponentInChildren<Gravity>();
        float lastDistance;

        do {
            lastDistance = Vector3.Distance(transform.position, target.position);
            yield return new WaitForFixedUpdate();
        } while (lastDistance >= Vector3.Distance(transform.position, target.position) || gravityList.Contains(targetGravity));

        Debug.Log(gravityList.Contains(targetGravity));
        // GameOver
        GameOver();
    }
    IEnumerator SendRayCast() {
        RaycastHit hit;
        float distance = asteroid.GetTargetDistance() + Asteroid.CATCH_DISTANCE;

        while (true) {
            yield return new WaitForEndOfFrame();

            if (Physics.Raycast(transform.position, -transform.forward * distance, out hit)) {
                if (hit.transform.gameObject.Equals(asteroid.targetAsteroid.gameObject)) {
                    Debug.Log(transform.name);
                }
            } 
        }
    }
    public void GameOver() {
        Debug.Log("Game Over");

        Camera.main.GetComponent<Cam>().GameOver(transform);

        Utility.isGameOver = true;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "o2small") {
            AddO2(O2_SMALL);
            Destroy(other.gameObject);
        }else if (other.tag == "o2big") {
            AddO2(O2_BIG);
            Destroy(other.gameObject);
        }
    }
}