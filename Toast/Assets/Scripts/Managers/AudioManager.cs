using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioPlayer;

    public AudioClip eatingBread;
    public AudioClip toasterTimer;
    public AudioClip toasterLever;
    public AudioClip toasterPop;
    public AudioClip toasterDing;
    public AudioClip objectiveComplete;
    public AudioClip fire;
    public AudioClip fireAlarm;

    private void Awake()
    {

        audioPlayer = GetComponent<AudioSource>();

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    public void PlaySound(AudioClip soundToPlay, float volume = 1, float delay =0)
    {
        if (soundToPlay == null)
        {
            return;
        }


        // example 1
        //audioPlayer.clip = soundToPlay;
        //audioPlayer.Play();
        Debug.Log(soundToPlay);
        // example 2
        audioPlayer.volume = volume;
        audioPlayer.PlayDelayed(delay);
        audioPlayer.PlayOneShot(soundToPlay);
    }
}
