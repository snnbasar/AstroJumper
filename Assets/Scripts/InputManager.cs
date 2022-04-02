using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
    public Player player;
    public Text ThrowSpeedText;
    public GameObject powerBarPanel, powerBar;
    float throwSpeed;
    bool throwing;
    Coroutine listenerCo;
    public static readonly float SPEED_PER_SEC = 20f, THROW_SPEED_LOWER_LIMIT = 10f, THROW_SPEED_UPPER_LIMIT = 50f, BAR_LIMIT = 940f;
    // Start is called before the first frame update
    void Start() {
        throwing = false;

        StartCoroutine(InputListenerTrigger(false));
    }

    // Update is called once per frame
    void Update() {
        
    }
    IEnumerator InputListenerTrigger(bool isActive) {
        Utility.isInputActive = isActive;

        while (true) {
            // Açılana kadar bekle
            yield return new WaitUntil(() => Utility.isInputActive);

            // Open
#if UNITY_EDITOR
            listenerCo = StartCoroutine(MouseListener());
#else
            listenerCo = StartCoroutine(TouchListener()); 
#endif

            // Kapanana kadar bekle
            yield return new WaitUntil(() => !Utility.isInputActive);

            // Close
            if (listenerCo != null)
                StopCoroutine(listenerCo);
        }
    }
    IEnumerator MouseListener() {
        while (true) {
            // Basana kadar bekle
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            throwing = true;

            StartCoroutine(ThrowSpeed());

            // Bırakana kadar bekle
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            throwing = false;
        }
    }
    IEnumerator TouchListener() {
        while (true) {
            // Basana kadar bekle
            yield return new WaitUntil(() => Input.touchCount > 0);

            yield return new WaitUntil(() => Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved);
            throwing = true;

            StartCoroutine(ThrowSpeed());

            // Bırakana kadar bekle
            yield return new WaitUntil(() => Input.GetTouch(0).phase == TouchPhase.Ended);
            throwing = false;
        }
    }
    IEnumerator ThrowSpeed() {
        throwSpeed = 0;

        // Show UI
        //ThrowSpeedText.gameObject.SetActive(true);
        powerBarPanel.SetActive(true);
        // Update UI
        //ThrowSpeedText.text = GetThrowSpeed().ToString();
        UpdatePowerBar();

        while (throwing && !Utility.isGameOver) {
            yield return new WaitForEndOfFrame();
            throwSpeed += SPEED_PER_SEC * Time.deltaTime;

            // Update UI
            //ThrowSpeedText.text = GetThrowSpeed().ToString();
            UpdatePowerBar();
        }

        // Hide UI
        //ThrowSpeedText.gameObject.SetActive(false);
        powerBarPanel.SetActive(false);

        // Throw
        if(!Utility.isGameOver)
            Throw(GetThrowSpeed());
    }
    public float GetThrowSpeed() {
        float gap = (THROW_SPEED_UPPER_LIMIT - THROW_SPEED_LOWER_LIMIT);
        bool isEven = ((int)(throwSpeed / gap)) % 2 == 0;
        float currentPower = throwSpeed % gap;

        return THROW_SPEED_LOWER_LIMIT + (isEven ? currentPower : gap - currentPower);
    }
    public void UpdatePowerBar() {
        float newY = BAR_LIMIT * (GetThrowSpeed() - THROW_SPEED_LOWER_LIMIT) / (THROW_SPEED_UPPER_LIMIT - THROW_SPEED_LOWER_LIMIT);
        powerBar.GetComponent<RectTransform>().sizeDelta = new Vector2(400, newY);
    }
    public void Throw(float throwSpeed) {
        player.Throw(throwSpeed);
    }
}