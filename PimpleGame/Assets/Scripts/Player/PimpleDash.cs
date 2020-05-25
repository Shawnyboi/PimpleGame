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

  [SerializeField] float pimpleSpotDistance = 5;
  [SerializeField] float pimpleSpotAngle = 90;

  [SerializeField] PimpleSpawner pimpleSpawner = null;

  private void Update()
  {
    target = null;
    var dashTo = transform.forward;

    if (allowInput)
    {
      if (Input.GetAxis("Fire1") > 0)
      {
        if (requireTarget && pimpleSpawner != null)
        {
          var pimples = pimpleSpawner.Pimples;
          float minCos = Mathf.Cos(pimpleSpotAngle);
          float nearestSqrDist = -1;

          foreach (var pimple in pimples)
          {
            if (pimple != null)
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
                    target = pimple.transform;
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
          AllowInput(false);
        }
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
