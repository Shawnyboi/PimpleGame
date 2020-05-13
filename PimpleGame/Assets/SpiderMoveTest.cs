using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMoveTest : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotSpeed = 30f;

    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        Vector3 newPos = this.transform.position + moveSpeed * Time.deltaTime * forwardInput * this.transform.forward;
        Quaternion rot = Quaternion.AngleAxis(turnInput * rotSpeed * Time.deltaTime, this.transform.up);
        Vector3 newForward = rot * this.transform.forward;

        this.transform.position = newPos;
        this.transform.forward = newForward;

    }
}
