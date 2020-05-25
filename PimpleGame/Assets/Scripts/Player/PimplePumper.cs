using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PimplePumper : MonoBehaviour
{
  PimpleInteract pimple = null;
  bool pumping = false;
  [SerializeField] JuicePouch pouch = null;

  [SerializeField] UnityEvent onHitPimple = null;
  [SerializeField] UnityEvent unHitPimple = null;

  private void Update()
  {
    if (pimple != null)
    {
      if (Input.GetAxis("Fire1") > 0)
      {
        if (!pumping)
        {
          pimple.pumpStart();
          pumping = true;
        }
        pouch.AddJuice(pimple.PumpingAmount);

        //TODO store pimple juice
      }
      else
      {
        pimple.pumpStop();
        pumping = false;
      }
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Pimple"))
    {
      pimple = collision.collider.GetComponentInParent<PimpleInteract>();
      onHitPimple.Invoke();
    }
  }

  private void OnCollisionExit(Collision collision)
  {
    if (pimple != null &&
      collision.gameObject.layer == LayerMask.NameToLayer("Pimple") 
      & collision.gameObject.GetComponentInChildren<PimpleInteract>() == pimple)
    {
      pimple = null;
      unHitPimple.Invoke();
    }
  } 
}
