﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpPimpAudio : MonoBehaviour
{
    Transform player;
    AudioSource PimpSource;
    public AudioClip PimpClip;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        PimpSource = GetComponent<AudioSource>();
    }
    public void PumpPimp()
    {
        PimpSource.pitch += 0.2f;
        transform.position = player.position;
        PimpSource.PlayOneShot(PimpClip);
    }
    public void ResetPitch()
    {
        PimpSource.pitch = 0.5f;
    }
}