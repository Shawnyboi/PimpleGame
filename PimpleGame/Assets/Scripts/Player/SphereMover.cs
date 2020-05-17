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
  [SerializeField] float turnSpeed = 10f;
  [SerializeField, Range(1, 100)] float turnRadius = 1; 

  Vector2 cachedInputMovement = Vector2.up;

  [SerializeField] SphereCollider ground = null;
  [SerializeField] Transform feet = null;
  [SerializeField] Transform desiredLook = null;
  [SerializeField] Transform actualLook = null;

  //[SerializeField] CinemachineVirtualCamera virtualCamera = null;
  [SerializeField] Camera camera = null;

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

      //var desiredLookTarget = 
      //lookTarget += 
      //inputMovement = cachedInputMovement + (inputMovement - cachedInputMovement)

      var cachedRight = new Vector2(cachedInputMovement.y, -cachedInputMovement.x);
      var cos = Mathf.Clamp(Vector3.Dot(inputMovement, cachedInputMovement), -1, 1);
      
      var angle = Mathf.Acos(cos) * Mathf.Rad2Deg;
      if (Vector3.Dot(cachedRight, inputMovement) < 0)
      {
        angle *= -1;
      }


      // TODO In think there is some gimbal lock happening either here or in the other rotation
      // TODO or something is getting fucked up by the rigidbody

      desiredLook.rotation = Quaternion.identity;
      desiredLook.transform.Rotate(new Vector3(0, angle, 0), Space.Self);

      var currentLookTarget = actualLook.transform.forward * turnRadius;
      var desiredLookTarget = desiredLook.forward * turnRadius;
      var turnAmount = desiredLookTarget - currentLookTarget;
      if (turnAmount.sqrMagnitude > 1)
      {
        turnAmount.Normalize();
      }
      var newLookTarget = currentLookTarget + turnAmount;

      float turnPortion = 0f;
      var desiredTurn = (desiredLookTarget - currentLookTarget).magnitude;
      var actualTurn = Mathf.Min((newLookTarget - currentLookTarget).magnitude, desiredTurn);

      if (desiredTurn > 0)
      {
        turnPortion = Mathf.Min(actualTurn / desiredTurn, 1);
      }
      turnPortion = 1;//todo remove

      //Debug.Log(cachedInputMovement + " " + inputMovement + " " + turnPortion + " " + angle);

      inputMovement = cachedInputMovement + ((inputMovement - cachedInputMovement) * turnPortion);
      inputMovement.Normalize();

      angle *= turnPortion;

      //var groundCenterToFeet = (feet.transform.position - ground.transform.position).normalized;
      //var right = transform.right;
      //transform.LookAt(body.position + Vector3.Cross(right, groundCenterToFeet), groundCenterToFeet);


      Vector3 force = Vector3.zero;
      //if (inputMovement.y > 0)
      {
        force = acceleration * actualLook.transform.forward;
      }
      //transform.Rotate(new Vector3(0, turnSpeed * inputMovement.x * Time.deltaTime, 0), Space.Self);
      actualLook.transform.Rotate(new Vector3(0, angle/* * Time.deltaTime*/, 0), Space.Self);

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

    /*var camRight = camera.transform.right;
    var camUp = Vector3.Cross(-groundCenterToFeet, camRight);
    camRight = Vector3.Cross(camUp, -groundCenterToFeet);
    camera.transform.position = groundCenterToFeet.normalized * 50;
    camera.transform.LookAt(camera.transform.position + Vector3.Cross(camRight, camUp), camUp);*/

    //virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = groundCenterToFeet.normalized * 20;

    var feetToBody = body.position - feet.transform.position;
    body.position = (groundCenterToFeet * (ground.transform.lossyScale.x * ground.radius)) + feetToBody;
  }
}
