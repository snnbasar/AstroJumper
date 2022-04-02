using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public GameObject[] musics;
    AudioSource musicSource;
    public int playingNow;
    public float volume;
    public Slider slider;
    void Start()
    {
        /*slider.minValue = 0f;
        slider.maxValue = 1f;*/
        MusicPlayer();
    }
    private void Update()
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
        
    }
    void MusicPlayer()
    {
        int tempMusic = Random.Range(0, musics.Length);
        musicSource = musics[tempMusic].GetComponent<AudioSource>();
        musicSource.Play();
        playingNow = tempMusic;
    }
}
