using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // ------------------------------- Variables
    [Header("Instance")]
    public static AudioManager instance;

    public AudioSource audioPlayer;

    [Header("Volume")]
    [SerializeField] 
    public float volumeMultiplier = 0.8f;

    [Header("-------- Audio Clips --------")]
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
    public AudioClip jamOpen;
    public AudioClip jamSplash;
    public AudioClip jamEat;
    public AudioClip pickUp;
    public AudioClip drop;
    public AudioClip lightOnFire;
    public AudioClip onFireAmbience;
    public AudioClip silence;
    public AudioClip sizzle;
    public AudioClip extinguish;

    public List<AudioSource> eventSources;
    private int iterator = 0;

    private float audioCd = 0.1f;


    // ------------------------------- Functions -------------------------------
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

        for(int i = 0; i < 10; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.dopplerLevel = 0.0f;
            eventSources.Add(source);
        }
    }

    private void Update()
    {
        audioCd -= Time.deltaTime;
    }

    /// <summary>
    /// Plays a sound a single time
    /// </summary>
    /// <param name="soundToPlay">Clip to be played</param>
    /// <param name="volume">Volume to play it at</param>
    /// <param name="delay">Delay</param>
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

    /// <summary>
    /// Play a given sound
    /// </summary>
    /// <param name="soundToPlay"></param>
    /// <param name="volume"></param>
    /// <param name="delay"></param>
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

    /// <summary>
    /// Sets global volume by reading the UI element
    /// </summary>
    public void ChangeVolume()
    {
        volumeMultiplier = UIManager.instance.volumeSlider.value;
        if(audioCd <= 0)
        {
            audioPlayer.volume = volumeMultiplier;
            audioPlayer.PlayOneShot(eatingBread);
            audioCd = 0.45f;
        }
    }

    public void PlayAudioEvent(SimpleAudioEvent audioEvent)
    {
        iterator = (iterator + 1) % 10;
        audioEvent.Play(eventSources[iterator]);
    }
}
