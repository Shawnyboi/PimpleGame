using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimplePumper : MonoBehaviour
{
  PimpleInteract pimple = null;
  bool pumping = false;
  [SerializeField] JuicePouch pouch = null;


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
    }
  }

  private void OnCollisionExit(Collision collision)
  {
    if (pimple != null &&
      collision.gameObject.layer == LayerMask.NameToLayer("Pimple") 
      & collision.gameObject.GetComponentInChildren<PimpleInteract>() == pimple)
    {
      pimple = null;
    }
  } 
}
