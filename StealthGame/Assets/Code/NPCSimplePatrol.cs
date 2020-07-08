using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSimplePatrol : MonoBehaviour
{
    [SerializeField]
    bool _patrolWaiting; //Whether the agent waits at a waypoint or not

    [SerializeField]
    float _totalWaitTime = 3f; //How long to wait 

    [SerializeField]
    float _SwitchProbability; //The chance to chance direction

    [SerializeField]
    List<Waypoint> _patrolPoints; //The list of patrol points to go though

    NavMeshAgent _navMeshAgent; 
    int _currentPatrolIndex;
    bool _traveling;
    bool _waiting;
    bool _patrolForward;
    float _waitTimer;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        if(_navMeshAgent == null)
        {
            Debug.LogError("No nav mesh agent attached to " + gameObject.name);
        }
        else
        {
            if(_patrolPoints != null && _patrolPoints.Count >=2)
            {
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.LogError("Not enought patrol points");
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        //Check to see if we are close to the destination
        if(_traveling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _traveling = false;

            //if we are going to wait the wait
            if(_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        //If we are waiting
        if(_waiting)
        {
            _waitTimer += Time.deltaTime;
            if(_waitTimer >= _totalWaitTime)
            {
                _waiting = false;

                ChangePatrolPoint();
                SetDestination();
            }
        }
    }

    private void ChangePatrolPoint()
    {
        /* // this code will allow the NPC to switch direction in the list
         //Generate a random number, if it is less than the switch probability then change direction
         if(UnityEngine.Random.Range(0f, 1f) <= _SwitchProbability)
         {
             _patrolForward = !_patrolForward;
         }

         //if we are moving foreward through the list
         if(_patrolForward)
         {
             //The % (Modulus division) will ensure that we dont go off the end of the list
             _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
         }
         else
         {
             //we are moving backwards so check that we dont fall off the bottom
             if(--_currentPatrolIndex < 0)
             {
                 _currentPatrolIndex = _patrolPoints.Count - 1;
             }
         }
     */

        //This will only have the NPC move forward through the list
        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
    }

    private void SetDestination()
    {
        //Cehck to make sure there are partol points
        if(_patrolPoints != null)
        {
            //Change the destination to the next patrol point
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _traveling = true;
        }
    }
}
