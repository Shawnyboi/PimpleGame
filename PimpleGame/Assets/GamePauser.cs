using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauser : MonoBehaviour
{
    public delegate void PauseAction();
    public static event PauseAction OnGamePause;

    public delegate void UnPauseAction();
    public static event UnPauseAction OnGameUnpause;

    public void pause()
    {
        if(OnGamePause != null)
        {
            OnGamePause();
        }
    }

    public void unpause()
    {
        if(OnGameUnpause != null)
        {
            OnGameUnpause();
        }
    }

}
