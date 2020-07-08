using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IdleState", menuName = "Unity-FSM/States/Idle", order = 1)]
public class IdleState : AbstractFSMState
{
    public override bool EnterState()
    {
        base.EnterState();
        
        Debug.Log("Entered idle state");
        return true;
    }
    public override void UpdateState()
    {
        Debug.Log("Updateing idle state");
    }

    public override bool ExitState()
    {
        base.ExitState();
        
        Debug.Log("Exiting idle state");
        return true;
    }
}
