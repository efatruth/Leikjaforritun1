using System.Linq;
using UnityEngine;

public class FirstPersonAudio : MonoBehaviour
{
    public FirstPersonMovement character;
    public GroundCheck groundCheck;

    [Header("Step")]
    public AudioSource stepAudio;
    public AudioSource runningAudio;
    [Tooltip("Minimum velocity for moving audio to play")]
    /// <summary> "Minimum velocity for moving audio to play" </summary>
    public float velocityThreshold = .01f;
    Vector2 lastCharacterPosition;
    Vector2 CurrentCharacterPosition => new Vector2(character.transform.position.x, character.transform.position.z);

    [Header("Landing")]
    public AudioSource landingAudio;
    public AudioClip[] landingSFX;

    [Header("Jump")]
    public Jump jump;
    public AudioSource jumpAudio;
    public AudioClip[] jumpSFX;

    [Header("Crouch")]
    public Crouch crouch;
    public AudioSource crouchStartAudio, crouchedAudio, crouchEndAudio;
    public AudioClip[] crouchStartSFX, crouchEndSFX;

    AudioSource[] MovingAudios => new AudioSource[] { stepAudio, runningAudio, crouchedAudio };


    void Reset()
    {
        // Setup stuff.
        // Uppsetningarefni.
        character = GetComponentInParent<FirstPersonMovement>();
        groundCheck = (transform.parent ?? transform).GetComponentInChildren<GroundCheck>();
        stepAudio = GetOrCreateAudioSource("Step Audio");
        runningAudio = GetOrCreateAudioSource("Running Audio");
        landingAudio = GetOrCreateAudioSource("Landing Audio");

        // Setup jump audio.
        // Setja upp hoppa hljóð.
        jump = GetComponentInParent<Jump>();
        if (jump)
        {
            jumpAudio = GetOrCreateAudioSource("Jump audio");
        }

        // Setup crouch audio.
        // Setja upp crouch hljóð.
        crouch = GetComponentInParent<Crouch>();
        if (crouch)
        {
            crouchStartAudio = GetOrCreateAudioSource("Crouch Start Audio");
            crouchStartAudio = GetOrCreateAudioSource("Crouched Audio");
            crouchStartAudio = GetOrCreateAudioSource("Crouch End Audio");
        }
    }

    void OnEnable() => SubscribeToEvents();

    void OnDisable() => UnsubscribeToEvents();

    void FixedUpdate()
    {
        // Play moving audio if the character is moving and on the ground.
        // Spilaðu hljóð sem hreyfist ef persónan er á hreyfingu og á jörðinni.
        float velocity = Vector3.Distance(CurrentCharacterPosition, lastCharacterPosition);
        if (velocity >= velocityThreshold && groundCheck && groundCheck.isGrounded)
        {
            if (crouch && crouch.IsCrouched)
            {
                SetPlayingMovingAudio(crouchedAudio);
            }
            else if (character.IsRunning)
            {
                SetPlayingMovingAudio(runningAudio);
            }
            else
            {
                SetPlayingMovingAudio(stepAudio);
            }
        }
        else
        {
            SetPlayingMovingAudio(null);
        }

        // Remember lastCharacterPosition.
        // Mundu lastCharacterPosition.
        lastCharacterPosition = CurrentCharacterPosition;
    }


    /// <summary>
    /// Pause all MovingAudios and enforce play on audioToPlay.
    /// Gerðu hlé á öllum MovingAudios og framfylgja spilun á audioToPlay.
    /// </summary>
    /// <param name="audioToPlay">Audio that should be playing.</param>
    void SetPlayingMovingAudio(AudioSource audioToPlay)
    {
        // Pause all MovingAudios.
        // Gera hlé á öllum MovingAudios.
        foreach (var audio in MovingAudios.Where(audio => audio != audioToPlay && audio != null))
        {
            audio.Pause();
        }

        // Play audioToPlay if it was not playing.
        // Spilaðu audioToPlay ef það var ekki að spila.
        if (audioToPlay && !audioToPlay.isPlaying)
        {
            audioToPlay.Play();
        }
    }

    #region Play instant-related audios.
    void PlayLandingAudio() => PlayRandomClip(landingAudio, landingSFX);
    void PlayJumpAudio() => PlayRandomClip(jumpAudio, jumpSFX);
    void PlayCrouchStartAudio() => PlayRandomClip(crouchStartAudio, crouchStartSFX);
    void PlayCrouchEndAudio() => PlayRandomClip(crouchEndAudio, crouchEndSFX);
    #endregion

    #region Subscribe/unsubscribe to events.
    void SubscribeToEvents()
    {
        // PlayLandingAudio when Grounded.
        // PlayLandingAudio þegar það er jarðað.
        groundCheck.Grounded += PlayLandingAudio;

        // PlayJumpAudio when Jumped.
        // Spilaðu JumpAudio þegar hoppað var.
        if (jump)
        {
            jump.Jumped += PlayJumpAudio;
        }

        // Play crouch audio on crouch start/end.
        // Spilaðu crouch hljóð á crouch start/end
        if (crouch)
        {
            crouch.CrouchStart += PlayCrouchStartAudio;
            crouch.CrouchEnd += PlayCrouchEndAudio;
        }
    }

    void UnsubscribeToEvents()
    {
        // Undo PlayLandingAudio when Grounded.
        // Afturkalla PlayLandingAudio þegar það er jarðað.
        groundCheck.Grounded -= PlayLandingAudio;

        // Undo PlayJumpAudio when Jumped.
        // Afturkalla PlayJumpAudio þegar hoppað er.
        if (jump)
        {
            jump.Jumped -= PlayJumpAudio;
        }

        // Undo play crouch audio on crouch start/end.
        // Afturkalla spilun crouch audio á crouch start/end.
        if (crouch)
        {
            crouch.CrouchStart -= PlayCrouchStartAudio;
            crouch.CrouchEnd -= PlayCrouchEndAudio;
        }
    }
    #endregion

    #region Utility.
    /// <summary>
    /// Get an existing AudioSource from a name or create one if it was not found.
    /// Fáðu fyrirliggjandi AudioSource úr nafni eða búðu til eitt ef það fannst ekki.
    /// </summary>
    /// <param name="name">Name of the AudioSource to search for.</param>
    /// <returns>The created AudioSource.</returns>
    AudioSource GetOrCreateAudioSource(string name)
    {
        // Try to get the audiosource.
        // Reyndu að ná í hljóðgjafann.
        AudioSource result = System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == name);
        if (result)
            return result;

        // Audiosource does not exist, create it.
        // Hljóðgjafi er ekki til, búðu til hana.
        result = new GameObject(name).AddComponent<AudioSource>();
        result.spatialBlend = 1;
        result.playOnAwake = false;
        result.transform.SetParent(transform, false);
        return result;
    }

    static void PlayRandomClip(AudioSource audio, AudioClip[] clips)
    {
        if (!audio || clips.Length <= 0)
            return;

        // Get a random clip. If possible, make sure that it's not the same as the clip that is already on the audiosource.
        // Fáðu handahófskennda bút. Ef mögulegt er skaltu ganga úr skugga um að það sé ekki það sama og myndbandið sem er þegar á hljóðgjafanum.
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clips.Length > 1)
            while (clip == audio.clip)
                clip = clips[Random.Range(0, clips.Length)];

        // Play the clip.
        // Spila klippuna.
        audio.clip = clip;
        audio.Play();
    }
    #endregion 
}
