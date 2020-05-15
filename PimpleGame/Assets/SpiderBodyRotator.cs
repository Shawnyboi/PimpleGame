using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasCenterOfGravity : MonoBehaviour
{
    public Transform centerOfGravity;
    private int groundLayerMask;

    protected void init()
    {
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        if (centerOfGravity == null)
        {
            centerOfGravity = GameObject.FindGameObjectWithTag("Planet").transform;
        }

        if (centerOfGravity == null)
        {
            Debug.LogWarning("missing center of gravity on target " + this.name);
        }
    }

    protected Vector3 raycastToCenterOfGravity()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, centerOfGravity.transform.position - this.transform.position, out hit, 1f, groundLayerMask))
        {
            return hit.point;
        }
        else if (Physics.Raycast(this.transform.position, this.transform.position - centerOfGravity.transform.position, out hit, 1f, groundLayerMask))
        {
            return hit.point;
        }
        else
        {
            return this.transform.position;
        }
    }
}

public class SpiderBodyRotator : HasCenterOfGravity
{
    public float standingHeight;

    private void Awake()
    {
        init();
    }

    private void rotateToPerpendicularOfSphere()
    {
        this.transform.up = this.transform.position - centerOfGravity.position;
    }

    
    private void standAtHeightAboveSphere()
    {
        Vector3 surfacePoint = raycastToCenterOfGravity();
        this.transform.position = surfacePoint + standingHeight * (this.transform.position - centerOfGravity.position);
    }

    private void Update()
    {
        rotateToPerpendicularOfSphere();
        standAtHeightAboveSphere();

    }
}
