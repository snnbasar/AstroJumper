using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    public int introWait = 11;
    void Start()
    {
        StartCoroutine(IntroWait());
    }
    IEnumerator IntroWait()
    {
        yield return new WaitForSeconds(introWait);
        SceneManager.LoadScene(1);
    }
}
