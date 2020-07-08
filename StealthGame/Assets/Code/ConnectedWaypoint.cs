using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConnectedWaypoint : Waypoint
{

    [SerializeField]
    protected float _connectivityRadius = 50f;

    List<ConnectedWaypoint> _connections;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        //Make sure there are waypoints and they are tagged to create the list
        if(allWaypoints.Length == 0)
        {
            Debug.LogError("There are no objects with tag of Waypoint");
        }

        _connections = new List<ConnectedWaypoint>();

        //loop throught the waypoint list
        for(int i = 0; i < allWaypoints.Length; i++)
        {
            //Make sure that the object tagged waypoint actiually has a Connectedwaypoint object attached to it
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();

            if(nextWaypoint != null)
            {
                //if the two objects are close enough add to _connections List
                if(nextWaypoint != this && Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= _connectivityRadius)
                {
                    _connections.Add(nextWaypoint);
                }
            }
        }
    }

    public override void OnDrawGizmos()
    {
        //Draw the base waypoint 
        base.OnDrawGizmos();

        //draw the connectivity radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _connectivityRadius);
    }

    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        //make sure there are connections
        if(_connections.Count == 0)
        {
            //if there are no connections complain
            Debug.LogError("Insufficient waypoints");
            return null;
        }
        else if(_connections.Count == 1 && _connections.Contains(previousWaypoint))
        {
            //if there is only one connection and it is the previous one go back
            return previousWaypoint;
        }
        else
        {
            //randomly pick a waypoint within range
            ConnectedWaypoint nextWaypoint;
            int nextIndex;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, _connections.Count);
                nextWaypoint = _connections[nextIndex];
            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}
