using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
  [SerializeField] protected Transform follow = null;
  [SerializeField] protected Transform lookAt = null;
  [SerializeField, Tooltip("Distance along the Follow's negative forward vector.")] protected float followDistance = 10f;

  private void Update()
  {
    updatePosition();
    updateOrientation();
  }

  protected virtual void updatePosition()
  {
    if (follow)
    {
      transform.position = follow.position - (follow.forward * followDistance);
    }
  }

  protected virtual void updateOrientation()
  {
    if (lookAt != null)
    {
      transform.LookAt(lookAt);
    }
  }
}
