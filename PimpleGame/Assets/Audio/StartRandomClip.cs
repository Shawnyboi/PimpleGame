using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRandomClip : MonoBehaviour
{
    AudioSource mainAudio;

    void Start()
    {
        mainAudio = GetComponent<AudioSource>();
        float clipLenth = mainAudio.clip.length;
        float randomStartTime = Random.Range(0, clipLenth);
        mainAudio.time = randomStartTime;
        mainAudio.Play();
    }

}
