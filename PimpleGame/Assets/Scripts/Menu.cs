using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

  [SerializeField] bool quitOnEscape = false;

  private void Update()
  {
    if (quitOnEscape && Input.GetKeyDown(KeyCode.Escape))
    {
      Quit();
    }
  }

  public void GotoScene(string scene)
  {
    SceneManager.LoadScene(scene);
  }

  public void Quit()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
//elif UNITY_WEBPLAYER
//  Application.OpenURL(webplayerQuitURL);
#else
     Application.Quit();
#endif
  }
}
