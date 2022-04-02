using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public Player player;
    public TutorialPlayer playerPrefab;
    public Asteroid startAsteroid;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(TutorialPlayerSpawner());
    }

    // Update is called once per frame
    void Update() {

    }
    //IEnumerator AsteroidChecker() {
    //    Asteroid asteroid = player.GetCurrentAsteroid();

    //    while (true) {
    //        yield return new WaitWhile(() => asteroid == player.GetCurrentAsteroid());

    //        asteroid = player.GetCurrentAsteroid();
    //    }
    //}
    IEnumerator TutorialPlayerSpawner() {
        yield return new WaitForSeconds(2.5f);
        while (true) {
            //if (player.GetCurrentAsteroid() == null)
            startAsteroid.Grap(Instantiate(playerPrefab).GetComponent<TutorialPlayer>());
            //else
            //    player.GetCurrentAsteroid().Grap(Instantiate(playerPrefab).GetComponent<TutorialPlayer>());

            //float rand = Random.Range(2f);
            yield return new WaitForSeconds(6f);
        }
    }

}
