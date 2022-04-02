using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStation : MonoBehaviour
{
    public GameObject bitisTimelineObject;
    public GameObject bitisKarakter;
    public GameObject bitisCutSceneCam;
    public GameObject playerObj;
    public GameObject mainCam;
    private void Start()
    {
        bitisTimelineObject.SetActive(false);
        bitisCutSceneCam.SetActive(false);
        bitisKarakter.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        bitisTimelineObject.SetActive(true);
        mainCam.SetActive(false);
        playerObj.SetActive(false);
    }
}
