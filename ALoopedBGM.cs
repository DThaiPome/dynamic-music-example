using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that plays a song and loops it smoothly.
/// </summary>
public abstract class ALoopedBGM : MonoBehaviour
{
    /// <summary>
    /// Configure this player to play a certain song.
    /// </summary>
    /// <param name="song">the non-null song to play and loop.</param>
    /// <param name="loopSeconds">the time, greater than 0, at which the
    /// next loop of the song should be started.</param>
    public abstract void SetSong(AudioClip song, float loopSeconds);

    /// <summary>
    /// Check if a song has been set to this player. A song needs to be
    /// set using SetSong before it can be played.
    /// </summary>
    /// <returns>true if SetSong has been properly 
    /// used to set a song to play.</returns>
    public abstract bool IsSongSet();

    /// <summary>
    /// Begin playing this song, if it is set.
    /// </summary>
    public abstract void Play();

    /// <summary>
    /// Stop playing this music.
    /// </summary>
    public abstract void Stop();

    /// <summary>
    /// Set the volume of this music.
    /// </summary>
    /// <param name="volume">a volume level between 0 and 1, inclusive.</param>
    public abstract void SetVolume(float volume);

}
