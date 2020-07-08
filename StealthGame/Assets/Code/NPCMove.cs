using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField]
    Transform _desitination;

    NavMeshAgent _navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null)
        {
            Debug.LogError("No nav-mesh agent on " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
    }

    private void SetDestination()
    {
        if(_desitination != null)
        {
            Vector3 tartgetPosition = _desitination.transform.position;
            _navMeshAgent.SetDestination(tartgetPosition);
        }
        else
        {
            Debug.LogError("No Destination for " + gameObject.name);
        }

    }
}
