using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereMover : MonoBehaviour
{
  Rigidbody body = null;
  [SerializeField] float acceleration = 1f;
  [SerializeField] float topSpeed = 10f; 

  Vector2 cachedInputMovement = Vector2.up;

  [SerializeField] SphereCollider ground = null;
  [SerializeField] Transform feet = null;
  [SerializeField] Transform desiredLook = null;
  [SerializeField] Transform actualLook = null;

  [SerializeField] float dampening = 0.8f;

  private void Awake()
  {
    body = GetComponent<Rigidbody>();
    if (feet == null)
    {
      feet = transform; 
    }
    cachedInputMovement = new Vector2(desiredLook.forward.x, desiredLook.forward.z);
  }

  private void FixedUpdate()
  {
    Vector2 inputMovement = Vector2.zero;
    inputMovement.x = Input.GetAxis("Horizontal");
    inputMovement.y = Input.GetAxis("Vertical");

    if (inputMovement.sqrMagnitude > Helper.Epsilon)
    {
      inputMovement.Normalize();

      var cachedRight = new Vector2(cachedInputMovement.y, -cachedInputMovement.x);
      var cos = Mathf.Clamp(Vector3.Dot(inputMovement, cachedInputMovement), -1, 1);
      
      var angle = Mathf.Acos(cos) * Mathf.Rad2Deg;
      if (Vector3.Dot(cachedRight, inputMovement) < 0)
      {
        angle *= -1;
      }

      desiredLook.rotation = Quaternion.identity;
      desiredLook.transform.Rotate(new Vector3(0, angle, 0), Space.Self);

      Vector3 force = Vector3.zero;
      force = acceleration * actualLook.transform.forward;
      actualLook.transform.Rotate(new Vector3(0, angle, 0), Space.Self);

      var velocity = body.velocity + (force * Time.deltaTime);
      if (velocity.sqrMagnitude > topSpeed * topSpeed)
      {
        velocity = velocity.normalized * topSpeed;
      }

      body.velocity = velocity;


      cachedInputMovement = inputMovement;

    }
    else
    {
      body.velocity *= dampening;
    }

    stickToSphere();
  }

  private void stickToSphere()
  {
    var groundCenterToFeet = (feet.transform.position - ground.transform.position).normalized;
    var right = actualLook.transform.right;

    actualLook.transform.LookAt(body.position + Vector3.Cross(right, groundCenterToFeet), groundCenterToFeet);

    var feetToBody = body.position - feet.transform.position;
    body.position = (groundCenterToFeet * (ground.transform.lossyScale.x * ground.radius)) + feetToBody;
  }
}
