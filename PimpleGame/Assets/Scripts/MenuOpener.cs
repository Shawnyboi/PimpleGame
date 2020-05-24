using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : MonoBehaviour
{
  [SerializeField] Animator menu = null;
  [SerializeField] string showParam = "On";

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      ToggleMenu();
    }
  }

  public void ToggleMenu()
  {
    bool isOpen = menu.GetBool(showParam);
    menu.SetBool(showParam, !isOpen);
  }

}
