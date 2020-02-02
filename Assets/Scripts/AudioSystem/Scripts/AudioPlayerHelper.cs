using UnityEngine;

public class AudioPlayerHelper : MonoBehaviour
{
    public void ToggleAudio(bool isOn)
    {
        AudioPlayer.Instance.ToggleAudio(isOn);
    }
}
