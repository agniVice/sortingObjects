using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    public void Play(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.pitch = pitch;
        _audioSource.Play();
    }
}