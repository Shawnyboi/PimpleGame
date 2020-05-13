using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFeetController : MonoBehaviour
{
    public List<SpiderFoot> firstFeetToMove;
    public List<SpiderFoot> secondFeetToMove;
    public float stateInterval = .25f;

    private float timePassedInState = 0f;
    private void init()
    {
        foreach(SpiderFoot foot in firstFeetToMove)
        {
            foot.unstick();
        }
    }

    private void Start()
    {
        init();
    }

    private void swapFeetStates()
    {
        foreach (SpiderFoot foot in firstFeetToMove)
        {
            foot.swapFootState();
        }

        foreach (SpiderFoot foot in secondFeetToMove)
        {
            foot.swapFootState();
        }
    }

    private void Update()
    {
        //temp foot sticking interval control
        timePassedInState += Time.deltaTime;
        if (timePassedInState > stateInterval)
        {
            timePassedInState = 0f;
            swapFeetStates();
        }

    }
}
