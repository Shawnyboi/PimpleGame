using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAI : MonoBehaviour
{
    public SpiderMover mover;

    public float idleTime = 2f;
    public float walkTime = 2f;
    enum spiderState
    {
        idle,
        wandering,
        chasing,
        following
    }

    private spiderState currentState;
    
    private void Start()
    {
        currentState = spiderState.idle;
        StartCoroutine(spiderStateMachine());
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
            currentState = spiderState.wandering;

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
            currentState = spiderState.idle;

            yield return null;
        }
    }

    private IEnumerator handleChasingState()
    {
        yield return null;
    }

    private IEnumerator handleFollowingState()
    {
        yield return null;
    }

    private IEnumerator spiderStateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case spiderState.idle:
                    yield return StartCoroutine(handleIdleState());
                    break;
                case spiderState.wandering:
                    yield return StartCoroutine(handleWanderingState());
                    break;
                case spiderState.chasing:
                    yield return StartCoroutine(handleChasingState());
                    break;
                case spiderState.following:
                    yield return StartCoroutine(handleFollowingState());
                    break;
                default:
                    break;
            }
        }
        yield return null;
    }
}
