using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioPlayer;

    private void Awake()
    {

        audioPlayer = GetComponent<AudioSource>();

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip soundToPlay)
    {
        // example 1
        audioPlayer.clip = soundToPlay;
        audioPlayer.Play();

        // example 2
        //audioPlayer.PlayOneShot(soundToPlay);
    }
}
