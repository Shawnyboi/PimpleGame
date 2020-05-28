using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAudio : MonoBehaviour
{
    Transform Player;
    AudioSource DashSource;
    public AudioClip DashSound;
    private void Start()
    {
        Player = GameObject.Find("Player").transform;
        DashSource = GetComponent<AudioSource>();
    }

    public void PlayDash()
    {
        transform.position = Player.position;
        DashSource.PlayOneShot(DashSound);
    }
}
