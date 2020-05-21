using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighPassPingPong : MonoBehaviour
{
    AudioHighPassFilter highPass;

    // Start is called before the first frame update
    void Start()
    {
        highPass = GetComponent<AudioHighPassFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        highPass.cutoffFrequency = Mathf.PingPong(Time.time * 100, 600) + 200;
    }
}
