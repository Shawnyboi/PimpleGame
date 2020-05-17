using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFollowCamera : FollowCamera
{
  [SerializeField] SphereCollider sphere = null;

  protected override void updateOrientation()
  {
    if (lookAt != null)
    {
      var targetToSphere = (sphere.transform.position - lookAt.position).normalized;
      var camRight = transform.right;
      var camUp = Vector3.Cross(targetToSphere, camRight);
      camRight = Vector3.Cross(camUp, targetToSphere);
      transform.LookAt(transform.position + Vector3.Cross(camRight, camUp), camUp);
    }
  }
}
