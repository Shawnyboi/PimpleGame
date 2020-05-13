using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFoot : MonoBehaviour
{
    public Transform footGroundPoint;
    public Transform footTarget;

    public float minDistanceToMoveFoot = .5f;
    public float footMoveTime = .1f;
    public float verticalDisplacement = .5f;

    public AnimationCurve footVerticalMovement;

    private Vector3 lastPosition;
    private Vector3 nextPosition;

    private bool sticking;
    
    public void unstick() { sticking = false; }
    private float distanceToFootTarget()
    {
        return Vector3.Distance(footGroundPoint.position, footTarget.position);
    }
    private void init()
    {
        sticking = true;
        lastPosition = footGroundPoint.position;
        nextPosition = footGroundPoint.position;
        StartCoroutine(footLoop());
    }

    private void Awake()
    {
        init();
    }

    private void startMovingFoot()
    {
        sticking = false;
        nextPosition = footTarget.position;
        StartCoroutine(moveFoot());
    }

    private void stopMovingFoot()
    {
        sticking = true;
        lastPosition = nextPosition;
        StartCoroutine(stickToGround());
    }

    public void swapFootState()
    {
        if (sticking)
        {
            startMovingFoot();
        }
        else
        {
            stopMovingFoot();
        }
    }

    private IEnumerator footLoop()
    {
        if (sticking)
        {
            yield return StartCoroutine(stickToGround());
        }
        else
        {
            yield return StartCoroutine(moveFoot());
        }
    }

    private IEnumerator stickToGround()
    {
        Debug.Log("Starting stick to gound");
        while (sticking && distanceToFootTarget() < minDistanceToMoveFoot)
        {
            footGroundPoint.position = lastPosition;
            yield return null;
        }
        yield return null;
    }

    private IEnumerator moveFoot()
    {
        Debug.Log("starting Move Foot");
        float timePassedMoving = 0f;
        //only lift foot if moving far
        bool liftFoot = Vector3.Distance(lastPosition, nextPosition) > .01f;
        while (timePassedMoving < footMoveTime)
        {
            nextPosition = footTarget.position;
            float newX = Mathf.Lerp(lastPosition.x, nextPosition.x, timePassedMoving / footMoveTime);
            float newY = Mathf.Lerp(lastPosition.y, nextPosition.y, timePassedMoving / footMoveTime);
            float newZ = Mathf.Lerp(lastPosition.z, nextPosition.z, timePassedMoving / footMoveTime);

            //Assuming always on flat ground TEMPORARY SOLUTION
            if (liftFoot)
            {
                newY += verticalDisplacement * footVerticalMovement.Evaluate(timePassedMoving / footMoveTime);
            }

            footGroundPoint.position = new Vector3(newX, newY, newZ);

            timePassedMoving += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

}
