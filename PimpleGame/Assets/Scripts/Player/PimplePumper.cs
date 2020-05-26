using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PimplePumper : MonoBehaviour
{
  PimpleInteract pimple = null;
  bool pumping = false;
  bool lookingForPimple = false;

  [SerializeField] JuicePouch pouch = null;

  [SerializeField] UnityEvent onHitPimple = null;
  [SerializeField] UnityEvent onUnhitPimple = null;
  [SerializeField] UnityEvent onPumpStart = null;
  [SerializeField] UnityEvent onPumpStop = null;

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
          onPumpStart.Invoke();
        }

        pouch.AddJuice(pimple.PumpingAmount * Time.deltaTime);

        //TODO store pimple juice
      }
      else if (pumping)
      {
        pimple.pumpStop();
        onPumpStop.Invoke();
        pumping = false;
      }
    }
  }

  public void LookForPimple()
  {
    lookingForPimple = true;
  }

  public void EndPumping()
  {
    if (pimple != null)
    {
      pimple.pimple.onLancedAway.RemoveListener(EndPumping);
    }

    pumping = false;
    pimple = null;

    onUnhitPimple.Invoke();
    onPumpStop.Invoke();
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (lookingForPimple && collision.gameObject.layer == LayerMask.NameToLayer("Pimple"))
    {
      lookingForPimple = false;
      pimple = collision.collider.GetComponentInParent<PimpleInteract>();
      pimple.pimple.onLancedAway.AddListener(EndPumping);
      onHitPimple.Invoke();
    }
  }

  /*private void OnCollisionExit(Collision collision)
  {
    if (pimple != null && collision.gameObject.layer == LayerMask.NameToLayer("Pimple"))
    {
      var hitPimple = collision.collider.GetComponentInParent<PimpleInteract>();
      if (hitPimple == pimple)
      {
        pimple = null;
        onUnhitPimple.Invoke();
      }
    }
  }*/ 
}
