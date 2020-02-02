using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }

    private const string audioPlayerPrefabPath = "AudioPlayer";

    [SerializeField] private UIAudioData audioData = null;

    [Header("BGM")]
    public AudioSource bgmAudioSource;
    public AudioMixerGroup bgmAudioMixerGroup;

    [Header("SFX")]
    public AudioSource sfxAudioSource;
    public AudioMixerGroup sfxAudioMixerGroup;

    public UIAudioData AudioData { get { return audioData; } }

    [RuntimeInitializeOnLoadMethod]
    private static void InitializeAudioPlayer()
    {
        if (Instance == null)
        {
            var audioPlayerObject = Instantiate(Resources.Load<GameObject>(audioPlayerPrefabPath));
            if (audioPlayerObject)
            {
                audioPlayerObject.name = nameof(AudioPlayer);
                Instance = audioPlayerObject.GetComponent<AudioPlayer>();
                DontDestroyOnLoad(audioPlayerObject);
            }
        }
    }

    public void ToggleBGM(bool isOn)
    {
        bgmAudioSource.mute = !isOn;
    }

    public void ToggleSFX(bool isOn)
    {
        sfxAudioSource.mute = !isOn;
    }

    public void ToggleAudio(bool isOn)
    {
        ToggleBGM(isOn);
        ToggleSFX(isOn);
    }

    /// <summary>
    /// Plays an AudioClip.
    /// </summary>
    /// <param name="clip"></param>
    public void PlayOneShot(AudioClip clip, float volume = 1.0f)
    {
        if (clip)
        {
            sfxAudioSource?.PlayOneShot(clip, volume);
        }
    }

    public void ChangeBGMTrack(AudioClip clip)
    {
        if (bgmAudioSource.clip != clip)
        {
            bgmAudioSource.clip = clip;
            bgmAudioSource?.Play();
        }
    }
}
