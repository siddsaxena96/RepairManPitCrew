using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPointerAudio : MonoBehaviour, IPointerClickHandler
{
    public UISoundType soundType = UISoundType.Neutral;

    private Selectable selectable;

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var clip = AudioPlayer.Instance.AudioData?.PrimarySystemSounds.Tap;
        switch (soundType)
        {
            case UISoundType.Neutral:
            case UISoundType.Positive:
               //clip = AudioPlayer.Instance.AudioData?.PrimarySystemSounds.Tap;
                break;
            case UISoundType.Negative:
                clip = AudioPlayer.Instance.AudioData?.SecondarySystemSounds.NavigationCancel;
                break;
            case UISoundType.Hover:
                clip = AudioPlayer.Instance.AudioData?.PrimarySystemSounds.NavigationHoverTap;
                break;
            default:
                break;
        }

        if (selectable)
        {
            if (!selectable.IsInteractable())
            {
                clip = AudioPlayer.Instance.AudioData.SecondarySystemSounds.NavigationUnavailableSelection;
            }
        }

        if (clip)
        {
            AudioPlayer.Instance.PlayOneShot(clip);
        }
    }
}
