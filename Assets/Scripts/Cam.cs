using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
    public static readonly float MOVE_THRESHOLD = 0.1f, ROTATE_THRESHOLD = 0.515f, ANIMATION_SPEED = 40f;
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
    public void GameOver(Transform tr) {
        Vector3 normalized = tr.right * 5f + tr.forward * 5f + tr.up * 3f;
        transform.position = tr.position + normalized;
        transform.LookAt(tr);
        transform.SetParent(tr);
    }
    public void MoveTo(Vector3 newPosition, Vector3 normalizedDirection){
        StartCoroutine(Animate(newPosition, normalizedDirection));
    }
    public void MovesAway(Vector3 newPosition) {
        StartCoroutine(MoveTo(newPosition));
    }
    IEnumerator MoveTo(Vector3 newPosition) {
        do {
            yield return new WaitForEndOfFrame();
        }
        while (!Utility.Move(transform, newPosition));
    }
    IEnumerator Animate(Vector3 newPosition, Vector3 normalizedDirection){
        // Input'u kilitle
        Utility.isInputActive = false;

        Quaternion rotation = Quaternion.LookRotation(normalizedDirection);
        
        bool moveEnded, rotateEnded;
        do {
            yield return new WaitForEndOfFrame();
            rotateEnded = Utility.Rotate(Camera.main.transform, rotation, ANIMATION_SPEED, ROTATE_THRESHOLD);
            moveEnded = Utility.Move(Camera.main.transform, newPosition, ANIMATION_SPEED, MOVE_THRESHOLD);
        } while ((!moveEnded || !rotateEnded) && !Utility.isGameOver);

        Camera.main.transform.rotation = rotation;
        transform.position = newPosition;

        // Input'u aç
        if(!Utility.isGameOver)
            Utility.isInputActive = true;
    }
}