using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        GamePauser.OnGameUnpause += unpause;
        GamePauser.OnGamePause += pause;
    }

    private void OnDisable()
    {
        GamePauser.OnGameUnpause += unpause;
        GamePauser.OnGamePause -= pause;
    }
    public void pause()
    {

    }

    public void unpause()
    {

    }
}
