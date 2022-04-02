using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    public static bool isInputActive = true,
                        isGameOver = false,
                        isRevived = false,
                        isChoking = false;
    public static int currentStageIndex;
    //public static bool Rotate(Transform transform, Quaternion rotation, float speed, float threshold) {
    //    Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, rotation, Time.deltaTime * speed); // LerpUnclamped

    //    if (threshold > Quaternion.Angle(Camera.main.transform.rotation, rotation)) {
    //        //Camera.main.transform.rotation = rotation;
    //        return true;
    //    } else
    //        return false;
    //}
    //public static bool Move(Transform transform, Vector3 newPosition, float speed, float threshold) {
    //    transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPosition, Time.deltaTime * speed); // LerpUnclamped

    //    if (threshold > Vector3.Distance(transform.position, newPosition)) {
    //        //transform.position = newPosition;
    //        return true;
    //    } else
    //        return false;
    //}
    public static bool Move(Transform transform, Vector3 newPosition) {
        transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPosition, Time.deltaTime * 1f);

        return Time.deltaTime * 1f > Vector3.Distance(transform.position, newPosition);
    }
    public static bool Rotate(Transform transform, Quaternion rotation, float speed, float threshold) {
        Camera.main.transform.rotation = Quaternion.LerpUnclamped(Camera.main.transform.rotation, rotation, Time.deltaTime * 4f);
        Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, rotation, Time.deltaTime * speed); // LerpUnclamped

        if (threshold > Quaternion.Angle(Camera.main.transform.rotation, rotation)) {
            //Camera.main.transform.rotation = rotation;
            return true;
        } else
            return false;
    }
    public static bool Move(Transform transform, Vector3 newPosition, float speed, float threshold) {
        transform.position = Vector3.LerpUnclamped(Camera.main.transform.position, newPosition, Time.deltaTime * 4f);
        //transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPosition, Time.deltaTime * speed); // LerpUnclamped

        if (threshold > Vector3.Distance(transform.position, newPosition)) {
            //transform.position = newPosition;
            return true;
        } else
            return false;
    }
    public static void SaveStage(int index, StageState stageState) {
        EncryptedPlayerPrefs.SetInt(index.ToString() + "_stageState", (int)stageState);
    }
    public static StageState LoadStage(int index) {
        return (StageState)EncryptedPlayerPrefs.GetInt(index.ToString() + "_stageState", (int)StageState.LOCKED);
    }
}
