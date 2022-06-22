// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : State_TeamReturn.cs 
// Description : AiState for returning to friendly side
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_TeamReturn : AIState
{
    void AIState.Enter(Script_Agent agent)
    {
    }

    void AIState.Exit(Script_Agent agent)
    {
    }

    AIStateID AIState.GetId()
    {
        return AIStateID.FRIENDLY_RETURN;
    }

    void AIState.Update(Script_Agent agent)
    {
        // Arrive at friendly side
        agent.Arrive(agent.Manager.transform.position);

        // if back on friendly side then return back to idle
        if (agent.IsOnFriendySide())
        {
            agent.Manager.oneOnWayToJail = false;
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
    }
}
