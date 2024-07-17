using System;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public static Audio Instance { get; private set; }

    public bool IsSoundEnabled { get; private set; }
    public bool IsMusicEnabled { get; private set; }

    public AudioClip ElementSelected;
    public AudioClip DrumSpin;
    public AudioClip Win;
    public AudioClip Lose;
    public AudioClip Change;
    public AudioClip Error;
    public AudioClip Success;
    public AudioClip PickUp;
    public AudioClip Spin;
    public AudioClip Buy;
    public AudioClip SlotWin;

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private GameObject _soundPrefab;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Initialize();
    }
    private void UpdateAudio()
    {
        _audioMixer.SetFloat("MusicForce", -80 * Convert.ToInt32(!IsMusicEnabled));
        _audioMixer.SetFloat("SoundForce", -80 * Convert.ToInt32(!IsSoundEnabled));
    }
    private void Initialize()
    {
        IsSoundEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("Sound", 1));
        IsMusicEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("Music", 0));
        UpdateAudio();
    }
    public void Save()
    {
        PlayerPrefs.SetInt("Sound", Convert.ToInt32(IsSoundEnabled));
        PlayerPrefs.SetInt("Music", Convert.ToInt32(IsMusicEnabled));
    }
    public void OnMusicToggle()
    {
        IsMusicEnabled = !IsMusicEnabled;
        UpdateAudio();
        Save();
    }
    public void OnSoundToggle()
    {
        IsSoundEnabled = !IsSoundEnabled;
        UpdateAudio();
        Save();
    }
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        var sound = Instantiate(_soundPrefab);
        sound.GetComponent<SFX>().Play(clip, volume, pitch);
        Destroy(sound, clip.length + 0.2f);
    }
}
