using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManyLoopedBGM : AManyLoopedBGM
{
    /// <summary>
    /// All of the songs that will be played in-parallel, and
    /// their sources. The index of a song in this list is its 
    /// song ID.
    /// </summary>
    [SerializeField]
    private List<AudioClip> songs;
    private List<ALoopedBGM> sources;
    /// <summary>
    /// The first song that is played.
    /// </summary>
    [SerializeField]
    private int initialSongId;
    /// <summary>
    /// The time at which each of the songs are looped.
    /// </summary>
    [SerializeField]
    private float loopSeconds = 60;
    /// <summary>
    /// The master volume of all of the sources. No audio will
    /// play above this volume.
    /// </summary>
    [SerializeField]
    private float masterVolume = 1;
    /// <summary>
    /// The speed at which songs transition, in units of volume per
    /// second.
    /// </summary>
    [SerializeField]
    private float fadePerSecond = 1f;

    /// <summary>
    /// The volume values that correspond to each source. These
    /// will be used to fade songs in and out.
    /// </summary>
    private List<float> sourceVolumes;
    /// <summary>
    /// The current song actively being played at full volume.
    /// </summary>
    private int songId;

    void Awake()
    {
        this.sourceVolumes = this.InitVolumes();
        this.sources = this.InitSourceList();
    }

    //Set up the list of source volumes.
    private List<float> InitVolumes() {
        List<float> result = new List<float>();
        for(int i = 0; i < this.songs.Count; i++)
        {
            result.Add(i == this.initialSongId ? 1 : 0);
        }
        return result;
    }

    //Create a child GameObject with a LoopedBGM on it, for every
    //song in the list.
    private List<ALoopedBGM> InitSourceList()
    {
        List<ALoopedBGM> result = new List<ALoopedBGM>();
        foreach(AudioClip song in this.songs)
        {
            ALoopedBGM loopedBGM = this.AddNewLoopedBGM();
            loopedBGM.SetSong(song, this.loopSeconds);
            result.Add(loopedBGM);
        }
        return result;
    }

    //Create a GameObject with a LoopedBGM on it.
    private ALoopedBGM AddNewLoopedBGM()
    {
        GameObject g = new GameObject();
        g.transform.SetParent(this.transform);
        ALoopedBGM loopedBGM = g.AddComponent<LoopedBGM>();
        return loopedBGM;
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateSourceVolumes();
        this.SetSourceVolumes();
    }

    //If a song is actively playing, raise its volume. If a song is not playing,
    //lower its volume.
    private void UpdateSourceVolumes()
    {
        float dVolume = this.fadePerSecond * Time.deltaTime;
        for(int i = 0; i < this.sourceVolumes.Count; i++)
        {
            if (i == this.songId)
            {
                this.sourceVolumes[i] = Mathf.Clamp(this.sourceVolumes[i] + dVolume, 0, 1);
            } else
            {
                this.sourceVolumes[i] = Mathf.Clamp(this.sourceVolumes[i] - dVolume, 0, 1);
            }
        }
    }

    //Set the volume of each song to a proportion of the master volume.
    private void SetSourceVolumes()
    {
        for(int i = 0; i < this.sources.Count; i++)
        {
            this.sources[i].SetVolume(Mathf.Lerp(0, this.masterVolume, this.sourceVolumes[i]));
        }
    }

    public override void SwitchToSong(int songId)
    {
        this.songId = Mathf.Clamp(songId, 0, this.songs.Count - 1);
    }

    public override bool IsSongSet()
    {
        foreach(AudioClip song in this.songs)
        {
            if (song == null)
            {
                return false;
            }
        }
        return true;
    }

    public override void Play()
    {
        if (this.IsSongSet())
        {
            foreach (ALoopedBGM loop in this.sources)
            {
                loop.Play();
            }
        }
    }

    public override void Stop()
    {
        foreach (ALoopedBGM loop in this.sources)
        {
            loop.Stop();
        }
    }

    public override void SetVolume(float volume)
    {
        this.masterVolume = Mathf.Clamp(volume, 0, 1);
    }
}
