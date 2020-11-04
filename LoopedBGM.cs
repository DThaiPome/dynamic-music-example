using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopedBGM : ALoopedBGM
{
    /// <summary>
    /// This is what is set when "SetSong" is used.
    /// </summary>
    [SerializeField]
    private AudioClip songToPlay;

    /// <summary>
    /// The timestamp of this song at which the new loop will start.
    /// </summary>
    [SerializeField]
    private float loopSeconds;

    /// <summary>
    /// "currentSource" will play the main iteration of the song that
    /// the player hears. When "currentSource" begins to fade out,
    /// "nextSource" will fade in its own iteration. Then, once
    /// "currentSource" finishes playing completely, the two sources
    /// will be swapped - "nextSource" will become the new "currentSource".
    /// </summary>
    private AudioSource currentSource;
    private AudioSource nextSource;

    /// <summary>
    /// A value between 0 and 1 that represents the master volume of both
    /// sources -  even when fading in/out, the volume of either source will 
    /// never exceed this value.
    /// </summary>
    [SerializeField]
    private float masterVolume;

    /// <summary>
    /// The speed at which songs transition, in units of volume per
    /// second.
    /// </summary>
    [SerializeField]
    private float fadePerSecond;

    /// <summary>
    /// These are the volumes for each source as they fade in and out. They will
    /// be scaled down to fit within the master volume.
    /// </summary>
    private float currentSourceVolume;
    private float nextSourceVolume;

    /// <summary>
    /// Is the song stopped, playing normally, or transitioning
    /// between sources?
    /// </summary>
    private enum LoopedBGMState
    {
        Stopped, Playing, Transitioning
    }
    private LoopedBGMState currentState;

    //Create two audio sources to play audio.
    void Awake()
    {
        this.currentState = LoopedBGMState.Stopped;
        this.currentSource = this.gameObject.AddComponent<AudioSource>();
        this.nextSource = this.gameObject.AddComponent<AudioSource>();
        this.SetSong(this.songToPlay, this.loopSeconds);
    }

    //Do whatever needs to be done while the audio is playing or
    //transitioning, and set the audio source volumes.
    void Update()
    {
        switch(this.currentState)
        {
            case LoopedBGMState.Playing:
                this.PlayingUpdate();
                break;
            case LoopedBGMState.Transitioning:
                this.TransitioningUpdate();
                break;
            case LoopedBGMState.Stopped:
                //Do nothing!
                return;
            default:
                return;
        }
        this.SetSourceVolumes();
    }

    private void SetSourceVolumes()
    {
        this.currentSource.volume = Mathf.Lerp(0, this.masterVolume, this.currentSourceVolume);
        this.nextSource.volume = Mathf.Lerp(0, this.masterVolume, this.nextSourceVolume);
    }

    //Check if the current source needs to be faded out. If so, start
    //playing next source, and transition!
    private void PlayingUpdate()
    {
        if (this.currentSource.time >= this.loopSeconds)
        {
            this.nextSource.Play();
            this.currentState = LoopedBGMState.Transitioning;
        }
    }

    //Every frame, make currentSource quieter, and nextSource louder. Once
    //currentSource is muted and nextSource is at full volume, stop currentSource
    //and swap them.
    private void TransitioningUpdate()
    {
        float dVolume = this.fadePerSecond * Time.deltaTime;
        this.currentSourceVolume = Mathf.Clamp(this.currentSourceVolume - dVolume, 0, 1);
        this.nextSourceVolume = Mathf.Clamp(this.nextSourceVolume + dVolume, 0, 1);
        
        if (this.currentSourceVolume == 0 && this.nextSourceVolume == 1)
        {
            this.currentSource.Stop();
            //Swap them - t is just a temporary place to put one of them while swapping.
            AudioSource t = this.currentSource;
            this.currentSource = this.nextSource;
            this.nextSource = t;

            //Don't forget to switch the volume values, too!
            float v = this.currentSourceVolume;
            this.currentSourceVolume = this.nextSourceVolume;
            this.nextSourceVolume = v;

            this.currentState = LoopedBGMState.Playing;
        }
    }

    public override bool IsSongSet()
    {
        return this.songToPlay != null;
    }

    public override void Play()
    {
        this.currentState = LoopedBGMState.Playing;
        this.currentSourceVolume = 1;
        this.nextSourceVolume = 0;
        this.currentSource.Play();
    }

    public override void SetSong(AudioClip song, float loopSeconds)
    {
        if (song != null)
        {
            this.songToPlay = song;
            this.loopSeconds = loopSeconds;
            this.currentSource.clip = song;
            this.nextSource.clip = song;
        }
    }

    public override void SetVolume(float volume)
    {
        this.masterVolume = Mathf.Clamp(volume, 0, 1);
    }

    public override void Stop()
    {
        this.currentState = LoopedBGMState.Stopped;
        this.currentSource.Stop();
        this.nextSource.Stop();
    }
}
