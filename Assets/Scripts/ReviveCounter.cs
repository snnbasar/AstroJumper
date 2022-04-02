using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveCounter : MonoBehaviour {
    public Scene scene;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void GameOver() {
        Utility.isGameOver = true;
    }
}
