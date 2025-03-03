using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    private List<AudioClip> musicClips;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private float volume = 1f;

    private float lerpProgress = 1f;
    private bool fadeOut = false;

    int currentSong = 0;
    int nextSong = 0;

    private void Awake()
    {
        instance = this;   
    }

    private void Start()
    {
        SetMusicClip(0);
        musicSource.loop = true;
    }

    private void Update()
    {
        if (fadeOut)
        {
            lerpProgress -= Time.deltaTime;
            if(lerpProgress <= 0f)
            {
                lerpProgress = 0f;
                fadeOut = false;
                currentSong = nextSong;
                musicSource.Stop();
                musicSource.clip = musicClips[currentSong];
                musicSource.Play();
            }
        }
        else
        {
            if(lerpProgress < 1f)
            {
                lerpProgress += Time.deltaTime;
                if(lerpProgress > 1f)
                {
                    lerpProgress = 1f;
                }
            }
        }

        musicSource.volume = Mathf.Lerp(0.0f, volume, lerpProgress);
    }

    public void SetMusicClip(int i)
    {
        nextSong = i;
        fadeOut = true;
    }

    public void ForceMusicClip(int i)
    {
        currentSong = i;
        musicSource.clip = musicClips[i];
        musicSource.Play();
    }
}
