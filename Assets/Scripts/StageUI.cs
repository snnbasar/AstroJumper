using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour {
    public int index;
    public StageState stageState;
    public string sceneName;
    public Text text;
    void Start() {
        
    }
    public void Open() {
        if (stageState == StageState.LOCKED)
            return;

        Utility.currentStageIndex = index;
        SceneManager.LoadScene(sceneName);
    }
    public void Set(int index, StageState stageState, string sceneName) {
        this.index = index;
        this.stageState = stageState;
        this.sceneName = sceneName;

        text = GetComponentInChildren<Text>();

        if (stageState == StageState.LOCKED)
            return;

        if (index != 0) 
            text.text = (index).ToString();
        else {
            text.text = "Tutorial";
            text.fontSize = 50;
        }
    }
}
