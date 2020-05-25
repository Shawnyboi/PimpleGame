using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFootTarget : HasCenterOfGravity
{
    
    private void Awake()
    {
        init();
    }

    private void LateUpdate()
    {
        Vector3 newPos = raycastToCenterOfGravity();
        if(newPos != null)
        {
            this.transform.position = newPos;
        }
    }
}
