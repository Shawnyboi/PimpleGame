using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFollowOrientation : MonoBehaviour
{
  [SerializeField] Transform follow = null;
  [SerializeField] Camera cam = null;

  private void Update()
  {
    transform.position = follow.position;
    transform.LookAt(follow.position + cam.transform.forward, follow.up);
  }
}
