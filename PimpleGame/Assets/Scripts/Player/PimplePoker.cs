using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimplePoker : MonoBehaviour
{
  PimpleInteract pimple = null;

  private void Update()
  {
    if (pimple != null)
    {
      Debug.Log("Pimple " + pimple);
      if (Input.GetAxis("Fire1") > 0)
      {

      }
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Pimple"))
    {
      pimple = collision.collider.GetComponentInParent<PimpleInteract>();
    }
  }

  private void OnCollisionExit(Collision collision)
  {
    if (pimple != null &&
      collision.gameObject.layer == LayerMask.NameToLayer("Pimple") 
      & collision.gameObject.GetComponentInChildren<PimpleInteract>() == pimple)
    {
      pimple = collision.collider.GetComponentInParent<PimpleInteract>();
    }
  } 
}
