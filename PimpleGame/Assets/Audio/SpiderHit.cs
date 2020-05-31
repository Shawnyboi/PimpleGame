using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHit : MonoBehaviour
{
    Transform player;
    AudioSource spideySource;
    public AudioClip spideyClip;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        spideySource = GetComponent<AudioSource>();
    }
    public void SiperSounds()
    {
        float randPitch = Random.Range(1f, 1.2f);
        spideySource.pitch = randPitch;
        transform.position = player.position;
        spideySource.PlayOneShot(spideyClip);
    }
}
