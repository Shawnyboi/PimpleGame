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
        if (Physics.Raycast(this.transform.position, -normal(), out hit, 1f, groundLayerMask))
        {
            return hit.point;
        }
        else if (Physics.Raycast(this.transform.position, normal(), out hit, 1f, groundLayerMask))
        {
            return hit.point;
        }
        else
        {
            return this.transform.position;
        }
    }

    protected Vector3 normal()
    {
       return this.transform.position - centerOfGravity.transform.position;
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
        transform.LookAt(this.transform.position + Vector3.Cross(this.transform.right, normal().normalized), normal());
    }

    
    private void standAtHeightAboveSphere()
    {
        Vector3 surfacePoint = raycastToCenterOfGravity();
        this.transform.position = surfacePoint + standingHeight * (normal());
    }

    private void Update()
    {
        rotateToPerpendicularOfSphere();
        standAtHeightAboveSphere();

    }
}
