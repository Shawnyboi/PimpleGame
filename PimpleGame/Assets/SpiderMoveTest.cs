using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMoveTest : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotSpeed = 30f;

    private void testMoveUpdate() {
        float forwardInput = Input.GetAxis("Vertical");

        Vector3 newPos = this.transform.position + moveSpeed * Time.deltaTime * forwardInput * this.transform.forward;

        float turnInput = Input.GetAxis("Horizontal");
        this.transform.Rotate(this.transform.up, turnInput * rotSpeed * Time.deltaTime);


        this.transform.position = newPos;
    }

    private void Update()
    {
        testMoveUpdate();
    }
}
