using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioPlayer;
    [SerializeField] float volumeMultiplier = 0.8f;

    public AudioClip eatingBread;
    public AudioClip toasterTimer;
    public AudioClip toasterLever;
    public AudioClip toasterPop;
    public AudioClip toasterDing;
    public AudioClip objectiveComplete;
    public AudioClip fire;
    public AudioClip fireAlarm;
    public AudioClip physicalButton;
    public AudioClip requirementComplete;
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
        volumeMultiplier = UIManager.instance.volumeSlider.value;
    }

    public void PlayOneShotSound(AudioClip soundToPlay, float volume = 1, float delay =0)
    {
        if (soundToPlay == null)
        {
            return;
        }


        // example 1
        //audioPlayer.clip = soundToPlay;
        //audioPlayer.Play();
        // example 2
        audioPlayer.volume = volume * volumeMultiplier;
        audioPlayer.PlayDelayed(delay);
        audioPlayer.PlayOneShot(soundToPlay);
    }

    public void PlaySound(AudioClip soundToPlay, float volume = 1, float delay = 0)
    {
        if (soundToPlay == null)
        {
            return;
        }


        // example 1

        audioPlayer.volume = volume *volumeMultiplier;
        audioPlayer.PlayDelayed(delay);
        audioPlayer.clip = soundToPlay;
        audioPlayer.Play();

    }

    public void ChangeVolume()
    {
        volumeMultiplier= UIManager.instance.volumeSlider.value;
        audioPlayer.volume = volumeMultiplier;
        audioPlayer.clip = eatingBread;
        audioPlayer.Play();

    }

    
}
