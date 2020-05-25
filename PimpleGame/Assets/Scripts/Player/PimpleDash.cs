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
  Pimple target = null;

  [SerializeField] float pimpleSpotDistance = 5;
  [SerializeField] float pimpleSpotAngle = 90;

  [SerializeField] PimpleSpawner pimpleSpawner = null;

  private void Update()
  {
    var dashTo = transform.forward;

    if (allowInput && !dashing)
    {
      if (target != null)
      {
        target.onPopped.RemoveListener(EndDash);
      }

      target = null;

      if (Input.GetAxis("Fire1") > 0)
      {
        if (requireTarget && pimpleSpawner != null)
        {
          var pimples = pimpleSpawner.Pimples;
          float minCos = Mathf.Cos(pimpleSpotAngle);
          float nearestSqrDist = -1;

          foreach (var pimple in pimples)
          {
            if (pimple != null && pimple.ReadyToLance)
            {
              var toPimple = pimple.transform.position - transform.position;
              if (toPimple.sqrMagnitude <= pimpleSpotDistance * pimpleSpotDistance)
              {
                var pimplePlanarDireciton = (toPimple - Vector3.Project(toPimple, transform.up)).normalized;
                var cos = Vector3.Dot(transform.forward, pimplePlanarDireciton);
                if (cos >= minCos)
                {
                  if (nearestSqrDist < 0 || (toPimple.sqrMagnitude < nearestSqrDist))
                  {
                    nearestSqrDist = toPimple.sqrMagnitude;
                    target = pimple;
                    target.onPopped.AddListener(EndDash);
                    dashTo = pimplePlanarDireciton;
                  }
                }
              }
            }
          }
        }

        if (!requireTarget || target != null)
        {
          dashing = true;
          dashRemaining = dashTime;
          dashTo *= dashModifier;
          dashStarted.Invoke(dashTo);
        }
      }
    }


    if (dashing)
    {
      bool hasTarget = target != null && !target.Popped;

      if (dashRemaining > 0 || hasTarget)
      {
        dashRemaining -= Time.deltaTime;
      }
      else
      {
        EndDash();
      }
    }
  }

  public void EndDash()
  {
    EndDash(false);
  }

  public void EndDash(bool silent)
  {
    dashRemaining = 0;
    dashing = false;
    target = null;
    if (!silent)
    {
      dashEnded.Invoke();
    }
  }

  public void AllowInput(bool allow)
  {
    allowInput = allow;
  }
}
