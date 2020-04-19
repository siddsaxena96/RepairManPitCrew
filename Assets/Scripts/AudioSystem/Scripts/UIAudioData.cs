using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UISoundType
{
    Neutral,
    Positive,
    Negative,
    Hover
}

[CreateAssetMenu(fileName = "NewUIAudioData", menuName = "MAGES Studio/Audio System/UI Audio Data")]
public class UIAudioData : ScriptableObject
{
    [SerializeField] private HeroSounds heroSounds = null;
    [SerializeField] private PrimarySystemSounds primarySystemSounds = null;
    [SerializeField] private SecondarySystemSounds secondarySystemSounds = null;

    [SerializeField] private AudioClip correctAnswer = null;
    [SerializeField] private AudioClip wrongAnswer = null;

    public HeroSounds HeroSounds => heroSounds;
    public PrimarySystemSounds PrimarySystemSounds => primarySystemSounds;
    public SecondarySystemSounds SecondarySystemSounds => secondarySystemSounds;

    public AudioClip CorrectAnswerSound { get { return correctAnswer; } }    
    public AudioClip WrongAnswerSound { get { return wrongAnswer; } }    
}

[System.Serializable]
public class HeroSounds
{
    [SerializeField] private AudioClip[] heroDecorativeCelebrationVariants = null;
    [SerializeField] private AudioClip[] heroSimpleCelebrationVariants = null;

    public AudioClip HeroDecorativeCelebration { get { return heroDecorativeCelebrationVariants[Random.Range(0, heroDecorativeCelebrationVariants.Length)]; } }
    public AudioClip HeroSimpleCelebrationVariants { get { return heroSimpleCelebrationVariants[Random.Range(0, heroSimpleCelebrationVariants.Length)]; } }
}

[System.Serializable]
public class PrimarySystemSounds
{
    [SerializeField] private AudioClip[] tapVariants = null;
    [SerializeField] private AudioClip navigationBackwardSelection = null;
    [SerializeField] private AudioClip navigationBackwardSelectionMinimal = null;
    [SerializeField] private AudioClip navigationForwardSelection = null;
    [SerializeField] private AudioClip navigationForwardSelectionMinimal = null;
    [SerializeField] private AudioClip navigationHoverTap = null;
    [SerializeField] private AudioClip navigationSelectionCompleteCelebration = null;
    [SerializeField] private AudioClip stateChangeConfirmDown = null;
    [SerializeField] private AudioClip stateChangeConfirmUp = null;
    [SerializeField] private AudioClip uiCameraShuter = null;
    [SerializeField] private AudioClip uiLock = null;
    [SerializeField] private AudioClip uiUnlock = null;

    public AudioClip Tap { get { return tapVariants[Random.Range(0, tapVariants.Length)]; } }
    public AudioClip NavigationBackwardSelection => navigationBackwardSelection;
    public AudioClip NavigationBackwardSelectionMinimal => navigationBackwardSelectionMinimal;
    public AudioClip NavigationForwardSelection => navigationForwardSelection;
    public AudioClip NavigationForwardSelectionMinimal => navigationForwardSelectionMinimal;
    public AudioClip NavigationHoverTap => navigationHoverTap;
    public AudioClip NavigationSelectionCompleteCelebration => navigationSelectionCompleteCelebration;
    public AudioClip StateChangeConfirmDown => stateChangeConfirmDown;
    public AudioClip StateChangeConfirmUp => stateChangeConfirmUp;
    public AudioClip UICameraShuter => uiCameraShuter;
    public AudioClip UILock => uiLock;
    public AudioClip UIUnlock => uiUnlock;
}

[System.Serializable]
public class SecondarySystemSounds
{
    [SerializeField] private AudioClip[] alertErrorVariants = null;
    [SerializeField] private AudioClip alertErrorVariant1 = null;
    [SerializeField] private AudioClip alertErrorVariant2 = null;
    [SerializeField] private AudioClip alertErrorVariant3 = null;

    [SerializeField] private AudioClip navigationTransitionLeft = null;
    [SerializeField] private AudioClip navigationTransitionRight = null;
    [SerializeField] private AudioClip navigationUnavailableSelection = null;
    [SerializeField] private AudioClip navigationCancel = null;

    [SerializeField] private AudioClip uiLoading = null;
    [SerializeField] private AudioClip uiRefreshFeed = null;

    public AudioClip AlertErrorVariant { get { return alertErrorVariants[Random.Range(0, alertErrorVariants.Length)]; } }
    public AudioClip AlertErrorVariant1 => alertErrorVariant1;
    public AudioClip AlertErrorVariant2 => alertErrorVariant2;
    public AudioClip AlertErrorVariant3 => alertErrorVariant3;

    public AudioClip NavigationTransitionLeft => navigationTransitionLeft;
    public AudioClip NavigationTransitionRight => navigationTransitionRight;
    public AudioClip NavigationUnavailableSelection => navigationUnavailableSelection;
    public AudioClip NavigationCancel => navigationCancel;

    public AudioClip UILoading => uiLoading;
    public AudioClip UIRefreshFeed => uiRefreshFeed;
}
