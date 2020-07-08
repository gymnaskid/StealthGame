using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class NPCConnectedPatrol : MonoBehaviour
{
    [SerializeField]
    bool _partolWaiting;

    [SerializeField]
    float _totalWaitTime = 3f;

    [SerializeField]
    float _switchProbability = 0f;

    NavMeshAgent _navMeshAgent;
    ConnectedWaypoint _currentWaypoint;
    ConnectedWaypoint _previousWaypoint;

    bool _travelling;
    bool _waiting;
    float _waitTimer;
    int _waypointsVisited;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("Ther is no nav mesh agent attatched to " + gameObject.name);
        }
        else
        {
            GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

            if (allWaypoints.Length > 0)
            {
                while (_currentWaypoint == null)
                {
                    int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                    ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                    if (startingWaypoint != null)
                    {
                        _currentWaypoint = startingWaypoint;
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to Find Waypoints");
            }
        }
        SetDestination();
    }



    // Update is called once per frame
    void Update()
    {
        //Check to see if we are close to our destination
        if (_travelling && _navMeshAgent.remainingDistance <= 1f)
        {
            _travelling = false;
            _waypointsVisited++;

            //If we are going to wait that wait
            if (_partolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;

            }
            else
            {
                //Set the next Destination
                SetDestination();
            }
        }

        //If we are waiting
        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if(_waitTimer >= _totalWaitTime)
            {
                _waiting = false;
                SetDestination();
            }
        }
    } 
    
    
    
    private void SetDestination()
    {
        if(_waypointsVisited > 0)
        {
            ConnectedWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
            _previousWaypoint = _currentWaypoint;
            _currentWaypoint = nextWaypoint;
        }

        Vector3 targetVector = _currentWaypoint.transform.position;
        _navMeshAgent.SetDestination(targetVector);
        _travelling = true;
    }
}
