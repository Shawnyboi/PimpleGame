using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMover : MonoBehaviour
{

    public float moveSpeed;
    public float turnSpeed;
    public Transform body;
    public void moveForward(float timeDelta)
    {
        Vector3 newPos = this.transform.position + moveSpeed * Time.deltaTime * this.transform.forward;
        body.position = newPos;
    }

    public void turn(bool turningRight, float timeDelta)
    {
        if (turningRight)
        {
            this.transform.Rotate(this.transform.up, turnSpeed * Time.deltaTime);
        }
        else
        {
            this.transform.Rotate(this.transform.up, -turnSpeed * Time.deltaTime);
        }
    }
}
