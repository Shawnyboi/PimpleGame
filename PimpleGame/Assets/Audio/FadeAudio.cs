using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAudio : MonoBehaviour
{

    IEnumerator Start()
    {
        AudioListener.volume = 0;
        while (AudioListener.volume < 1)
        {
            AudioListener.volume += 0.01f;
            yield return new WaitForEndOfFrame();
        }
    }

    public void FadeOutAudio()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (AudioListener.volume > 0)
        {
            AudioListener.volume -= 0.01f;
            yield return new WaitForEndOfFrame();
        }
    }
}
