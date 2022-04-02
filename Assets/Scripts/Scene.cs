using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour {
    public Asteroid demoAsteroid;
    public Player player;
    public Transform startStation, endStation;
    public GameObject RevivePanel, GameOverPanel, star, nextButton, o2Panel, o2Bar;
    public GameObject prefabAstronaut;
    public Sprite[] starSprites = new Sprite[4];
    public static readonly float randomAstronautSpawnRangeUp = 10f, randomAstronautSpawnRangeForward = 20f, randomAstronautSpawnTime = 10f, BAR_LIMIT = 940f;
    Coroutine H2OConsumerCO;
    // Start is called before the first frame update
    void Start() {
        demoAsteroid.Grap(player);
        
        o2Panel.SetActive(true);
        nextButton.SetActive(false);

        Utility.isGameOver = false;
        Utility.isRevived = false;

        StartCoroutine(GameOverChecker());
        StartCoroutine(RandomAstronautSpawn());
        H2OConsumerCO = StartCoroutine(H2OConsumer());
    }

    // Update is called once per frame
    void Update() {

    }
    IEnumerator GameOverChecker() {
        yield return new WaitUntil(() => Utility.isGameOver);

        player.GetComponentInChildren<Animator>().SetBool("isDead", true);
        OpenRevivePanel();
    }
    private void OpenRevivePanel() {
        if (!Utility.isRevived) 
            RevivePanel.SetActive(true);
        else
            GameOver();
    }
    private void CloseRevivePanel() {
        RevivePanel.SetActive(false);
    }
    private int GetStars() {
        if (Utility.isChoking || Utility.isGameOver)
            return 0;

        // Eğer Bir sonraki stage kilitlityse, aç
        if (Utility.LoadStage(Utility.currentStageIndex + 1) == StageState.LOCKED)
            Utility.SaveStage((Utility.currentStageIndex + 1), StageState.ZERO_STAR);

        if (player.GetO2() > Player.O2_LIMIT * 60f / 100f)
            return 3;
        else if (player.GetO2() > Player.O2_LIMIT * 40f / 100f)
            return 2;
        else
            return 1;
    }
    public void GameOver() {
        StopCoroutine(H2OConsumerCO);

        Utility.SaveStage(Utility.currentStageIndex, (StageState)GetStars());

        if (Utility.isChoking)
            Choking();

        CloseRevivePanel();
        GameOverPanel.SetActive(true);

        int starCount = GetStars();

        if (starCount != 0)
            nextButton.SetActive(true);

        star.GetComponent<Image>().sprite = starSprites[starCount];
    }
    public void Home() {
        SceneManager.LoadScene("Menu");
    }
    public void Next() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReStart() {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
    public void Revive() {
        CloseRevivePanel();

        Utility.isRevived = true;
        player.GetComponentInChildren<Animator>().SetBool("isDead", false);
        Utility.isGameOver = false; 
        StartCoroutine(GameOverChecker());

        Camera.main.transform.parent = null;
        player.GetCurrentAsteroid().Grap(player);
    }
    public void Won() {
        StopCoroutine(H2OConsumerCO);
    }
    IEnumerator RandomAstronautSpawn() {
        Vector3 direction = (endStation.position - startStation.position).normalized;
        
        while (true) {
            yield return new WaitForSeconds(randomAstronautSpawnRangeUp);
            Vector3 position = startStation.position +
                                startStation.right * 2f +
                                startStation.up * Random.Range(-randomAstronautSpawnRangeUp, randomAstronautSpawnRangeUp) +
                                startStation.forward * Random.Range(-randomAstronautSpawnRangeForward, randomAstronautSpawnRangeForward);

            GameObject randomAstronaut = Instantiate(prefabAstronaut, position, Quaternion.identity);

            randomAstronaut.GetComponent<Rigidbody>().AddForce(direction * 20f, ForceMode.Impulse);
            randomAstronaut.transform.LookAt(startStation);

            Destroy(randomAstronaut, 20f);
        }
    }
    IEnumerator H2OConsumer() {
        while (player.ReduceO2(Time.deltaTime * Player.O2_PER_SEC)) {
            UpdateO2Bar();

            yield return new WaitForEndOfFrame();
        }

        // Camera moves away
        Utility.isRevived = true;
        Utility.isChoking = true;
        Utility.isGameOver = true;
    }
    public void Choking() {
        Vector3 direction = player.GetCurrentAsteroid().GetCamNormalizedDirection(Camera.main.transform.position);
        Camera.main.GetComponent<Cam>().MovesAway(Camera.main.transform.position - (5f * direction));

        player.GetComponentInChildren<Animator>().SetBool("Choking", true);
        player.Choked();
    }
    public void UpdateO2Bar() {
        float newY = BAR_LIMIT * (player.GetO2() - 0) / (Player.O2_LIMIT - 0);
        o2Bar.GetComponent<RectTransform>().sizeDelta = new Vector2(400, newY);
    }
}
