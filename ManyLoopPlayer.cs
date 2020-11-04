using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManyLoopPlayer : MonoBehaviour
{
    [SerializeField]
    private AManyLoopedBGM player;
    [SerializeField]
    private int songId = 0;

    void Start()
    {
        this.player.Play();
    }

    void Update()
    {
        this.player.SwitchToSong(this.songId);
    }
}
