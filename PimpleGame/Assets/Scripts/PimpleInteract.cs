using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PimpleInteract : MonoBehaviour
{
    //Dear Samward, 
    //I am writing this code thinking of you
    //I don't know exactly how it should work so I'm just gonna guess how it might be good. 
    //
    // Basically the idea is 
    // when you start holding the pump button call pumpStart
    // when you stop holding the pump button call pumpStop
    // if you hold too long it will stop reducing the pimple size so you have to let go and start the pump again
    // always do a null check before calling anything on this class because it gets deleted when the pimple size gets to zero
    // (*￣3￣)╭
    //luv,
    //Shawn


    public float pumpRate = 20f;
    public float reductionPerPump = 35f;

    private float amtReducedOnCurrentPump = 0f;

    private bool pumping;
    public Pimple pimple;

    public float PumpingAmount => pumping ? reductionPerPump : 0;

    private void init()
    {
        pimple = GetComponent<Pimple>();
        if(pimple == null)
        {
            Debug.LogError("Pimple Interact missing Pimple Script");
        }
    }

    private void Awake()
    {
        init();
    }
    public void pumpStart()
    {
        pumping = true;
        pimple.freezeGrowth();
    }

    public void pumpStop()
    {
        amtReducedOnCurrentPump = 0f;
        pumping = false;
        pimple.unfreezeGrowth();
    }

    private void pumpUpdate(float timeDelta)
    {
        amtReducedOnCurrentPump += Time.deltaTime * pumpRate;
        pimple.reduceSize(Time.deltaTime * pumpRate);
    }

    private void Update()
    {
        if (pumping)
        {
            if(amtReducedOnCurrentPump < reductionPerPump)
            {
                pumpUpdate(Time.deltaTime);
            }
            else
            {
                pumpStop();
            }
        }
    }
}
