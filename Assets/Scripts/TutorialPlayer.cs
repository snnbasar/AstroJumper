using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : Player {
    int asteroidCount;
    // Start is called before the first frame update
    void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        gravityList = new List<Gravity>();
        currentO2 = O2_LIMIT;
        asteroidCount = 0;
        //Destroy(gameObject, 35);
    }

    // Update is called once per frame
    void Update() {

    }
    public new void Throw(float throwSpeed) {
        if (SendRayCastCo != null)
            StopCoroutine(SendRayCastCo);

        isGrapping = false;

        transform.parent = null;
        rb.AddForce(-transform.forward * throwSpeed, ForceMode.Impulse);
        anim.SetBool("Zipla", true);
        savedO2 = currentO2;

        if (asteroidCount > 6)
            Destroy(gameObject, 1f);
        //flyingCo = StartCoroutine(Flying());
    }
    public new void Grap(GameObject __asteroid) {
        if (flyingCo != null)
            StopCoroutine(flyingCo);

        isGrapping = true;

        // Revived
        if (__asteroid.GetComponent<Asteroid>().Equals(asteroid))
            currentO2 = savedO2;

        asteroidCount++;
        //if (asteroidCount > 6)
        //    Destroy(gameObject);
        //if (asteroid != null)
        //    Destroy(gameObject);

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

        transform.SetParent(asteroid.transform);
        anim.SetBool("Zipla", false);

        SendRayCastCo = StartCoroutine(SendRayCast());
    }
    IEnumerator SendRayCast() {
        RaycastHit hit;
        float distance = asteroid.GetTargetDistance() + Asteroid.CATCH_DISTANCE;
        int count = 0;

        while (true) {
            yield return new WaitForEndOfFrame();

            if (Physics.Raycast(transform.position, -transform.forward * distance, out hit)) {
                if (hit.transform.gameObject.Equals(asteroid.targetAsteroid.gameObject)) {
                    count++;

                    //if(count >= 5)
                        Throw(30);
                } else
                    count = 0;
            }
        }
    }
}
