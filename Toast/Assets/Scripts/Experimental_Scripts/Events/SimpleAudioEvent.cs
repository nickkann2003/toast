using NaughtyAttributes;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Event", menuName = "Audio Event", order = 52)] // adds Audio Event as an asset in the asset menu
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    [MinMaxSlider(0,1)]
    public Vector2 volume;

    [MinMaxSlider(0,2)]
    public Vector2 pitch;

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];

        source.volume = Random.Range(volume.x, volume.y);
        source.pitch = Random.Range(pitch.x, pitch.y);
        source.clip = clip;

        source.Play();
    }
}
