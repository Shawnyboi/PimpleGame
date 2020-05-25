using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAI : MonoBehaviour
{
    public SpiderMover mover;

    public float idleTime = 2f;
    public float walkTime = 2f;
    public float followDistance = 5f;
    public float aggroDistance = 10f;

    public GameObject player;
    enum spiderState
    {
        idle,
        wandering,
        chasing,
        following
    }

    private spiderState currentState;
    
    void init()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        currentState = spiderState.idle;
        StartCoroutine(spiderStateMachine());
    }

    private float getDistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, mover.transform.position);
    }

    private IEnumerator handleIdleState()
    {
        float timePassed = 0f;
        int rightOrLeftRoll = Random.Range(0, 2);
        while (timePassed < idleTime)
        {
            timePassed += Time.deltaTime;
            mover.turn(rightOrLeftRoll == 0, Time.deltaTime);

            //determine next state
            if (getDistanceToPlayer() < aggroDistance)
            {
                currentState = spiderState.following;
            }
            else
            {
                currentState = spiderState.wandering;
            }



            yield return null;
        }
    }

    private IEnumerator handleWanderingState()
    {
        float timePassed = 0f;
        while (timePassed < walkTime)
        {
            timePassed += Time.deltaTime;
            mover.moveForward(Time.deltaTime);

            //determine next state
            if (getDistanceToPlayer() < aggroDistance)
            {
                currentState = spiderState.following;
            }
            else
            {
                currentState = spiderState.idle;
            }

            yield return null;
        }
    }

    private IEnumerator handleChasingState()
    {
        yield return lookInDirectionOfPlayer();
        float oldMoveSpeed = mover.moveSpeed;
        float oldWalkTime = walkTime;
        walkTime = walkTime / 2f;
        mover.moveSpeed = mover.moveSpeed * 2f;
        float timePassed = 0;
        while (timePassed < walkTime)
        {
            timePassed += Time.deltaTime;
            mover.moveForward(Time.deltaTime);
            yield return null;
        }
        currentState = spiderState.idle;
        mover.moveSpeed = oldMoveSpeed;
        walkTime = oldWalkTime;
        yield return null;
    }

  
    private bool playerIsOnRight()
    {
        Vector3 vToPlayer = player.transform.position - mover.transform.forward;
        Vector3 vFwd = mover.transform.forward;
        Vector3 vRight = mover.transform.right;

        return Vector3.Dot(vRight, vToPlayer) > 0;
    }

    private IEnumerator lookInDirectionOfPlayer()
    {
        bool playerWasOnRight = playerIsOnRight();
        while (playerWasOnRight == playerIsOnRight())
        {
            playerWasOnRight = playerIsOnRight();
            mover.turn(playerWasOnRight, Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator moveTowardPlayerKeepingDistance()
    {
        float timePassed = 0f;
        while(timePassed < walkTime && getDistanceToPlayer() > followDistance){
            timePassed += Time.deltaTime;
            mover.moveForward(Time.deltaTime);
            yield return null;
        }

        if(getDistanceToPlayer() < followDistance)
        {
            currentState = spiderState.chasing;
        }
        yield return null;
    }

    private IEnumerator handleFollowingState()
    {
       
        yield return lookInDirectionOfPlayer();
        yield return moveTowardPlayerKeepingDistance();
        yield return null;
    }

    private IEnumerator spiderStateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case spiderState.idle:
                    //Debug.Log("Handling Idle State");
                    yield return StartCoroutine(handleIdleState());
                    break;
                case spiderState.wandering:
                    //Debug.Log("Handling Wandering State");
                    yield return StartCoroutine(handleWanderingState());
                    break;
                case spiderState.chasing:
                    //Debug.Log("Handling Chasing State");
                    yield return StartCoroutine(handleChasingState());
                    break;
                case spiderState.following:
                    //Debug.Log("Handling Following state");
                    yield return StartCoroutine(handleFollowingState());
                    break;
                default:
                    break;
            }
            yield return null;
        }
        yield return null;
    }
}
