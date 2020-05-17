using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSticker : MonoBehaviour
{
  Rigidbody body = null;

  [SerializeField] bool autoStick = false;
  [SerializeField] SphereCollider sphere = null;
  [SerializeField] Transform feet = null;
  [SerializeField] Transform looker = null;

  private void Awake()
  {
    body = GetComponent<Rigidbody>();
    if (feet == null)
    {
      feet = transform;
    }
  }

  private void Update()
  {
    if (autoStick)
    {
      StickToSphere();
    }
  }

  public void StickToSphere()
  {
    var sphereCenterToFeet = (feet.transform.position - sphere.transform.position).normalized;
    var right = looker.transform.right;

    looker.transform.LookAt(position + Vector3.Cross(right, sphereCenterToFeet), sphereCenterToFeet);

    var feetToBody = position - feet.transform.position;
    position = (sphereCenterToFeet * (sphere.transform.lossyScale.x * sphere.radius)) + feetToBody;
  }

  Vector3 position
  {
    get {
      if (body != null)
      {
        return body.position;
      }
      else
      {
        return transform.position;
      }
    }
    set
    {
      if (body != null)
      {
        body.position = value;
      }
      else
      {
        transform.position = value;
      }
    }
  }
}
