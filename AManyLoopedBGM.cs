using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AManyLoopedBGM : ALoopedBGM
{
    //We will be setting the song through the inspector, so this
    //method can be nullified.
    public override void SetSong(AudioClip song, float loopSeconds)
    {
        return;
    }

    /// <summary>
    /// Switch the currently active song to the one with this
    /// song ID.
    /// </summary>
    /// <param name="songId">an ID between 0 and one less than the
    /// number of songs, inclusive.</param>
    public abstract void SwitchToSong(int songId);
}
