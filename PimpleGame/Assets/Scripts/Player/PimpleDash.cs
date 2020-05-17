using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PimpleDash : MonoBehaviour
{
  bool allowInput = true;
  bool dashing = false;
  float dashRemaining = 0;

  [SerializeField] float dashModifier = 2f;
  [SerializeField] float dashTime = 1f;

  [SerializeField] UnityEventVector3 dashStarted = null;
  [SerializeField] UnityEvent dashEnded = null;

  [SerializeField] bool requireTarget = true;
  Transform target = null;

  private void Update()
  {
    if (allowInput && (!requireTarget || target != null))
    {
      if (Input.GetAxis("Fire1") > 0)
      {
        dashing = true;
        dashRemaining = dashTime;
        var dashTo = (target != null ? (target.position - transform.position) : transform.forward) * dashModifier;
        dashStarted.Invoke(dashTo);
        AllowInput(false);
      }
    }


    if (dashing)
    {
      if (dashRemaining > 0)
      {
        dashRemaining -= Time.deltaTime;
      }
      else
      {
        AllowInput(true);
        dashEnded.Invoke();
      }
    }
  }

  public void AllowInput(bool allow)
  {
    allowInput = allow;
  }

  public void SetTarget(GameObject newTarget)
  {
    target = newTarget.transform;
  }
}
