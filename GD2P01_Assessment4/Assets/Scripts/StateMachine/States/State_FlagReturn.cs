// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : State_FlagReturn.cs 
// Description : AIState for returning a flag
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FlagReturn : AIState
{
    void AIState.Enter(Script_Agent agent)
    {
    }

    void AIState.Exit(Script_Agent agent)
    {
    }

    AIStateID AIState.GetId()
    {
        return AIStateID.FLAG_RETURN;
    }

    void AIState.Update(Script_Agent agent)
    {
        // Arrive at friendly side
        agent.Arrive(agent.Manager.transform.position);

        // Check if agent is on there own side
        if (agent.IsOnFriendySide())
        {
            // flag capturer arrived back
            agent.Manager.oneOnWayToFlag = false;
            // Attach flag to friendly flag holder
            if (agent.AttachedFlag != null)
                agent.AttachedFlag.Attach(agent.Manager.GetFriendlyFlagHolder());
            // Change state too idle
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
    }
}
