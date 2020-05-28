using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightAudio : MonoBehaviour
{
    AudioSource HLaudioSource;
    public AudioClip HLaudioClip;
    public AudioClip Select;
    void Start()
    {
        HLaudioSource = transform.Find("AudioHighlight").GetComponent<AudioSource>();
    }

    public void PlayHighLightAudio()
    {
        HLaudioSource.PlayOneShot(HLaudioClip);
    }
    public void PlayTheFuckingGameOrNot()
    {
        HLaudioSource.PlayOneShot(Select);

    }
}
