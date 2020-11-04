using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopedBGMPlayer : MonoBehaviour
{
    [SerializeField]
    private ALoopedBGM player;
    [SerializeField]
    private AudioClip song;
    [SerializeField]
    private float loopSeconds;
    [SerializeField]
    private float volume = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.player.SetSong(song, loopSeconds);
        this.player.Play();
    }

    // Update is called once per frame
    void Update()
    {
        this.player.SetVolume(volume);
    }
}
