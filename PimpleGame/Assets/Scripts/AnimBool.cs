using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimBool : MonoBehaviour
{
  Animator animator = null;

  public void SetTrue(string param)
  {
    if (animator == null)
    {
      animator = GetComponent<Animator>();
    }
    animator.SetBool(param, true);
  }

  public void SetFalse(string param)
  {
    if (animator == null)
    {
      animator = GetComponent<Animator>();
    }
    animator.SetBool(param, false);
  }
}
