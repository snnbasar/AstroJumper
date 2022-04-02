using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Stage")]
public class StageScriptable : ScriptableObject {
    public Color color;
    public string sceneName;
}
