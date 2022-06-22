// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : State_FreeTeamMate.cs 
// Description : AIState for freeing a teammate
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FreeTeamMate : AIState
{
    Script_Agent closestFriendly = null;
    public void Enter(Script_Agent agent)
    {
        // Get closest jailed friendly
        closestFriendly = agent.GetClosestJailedAgent();
    }

    public void Exit(Script_Agent agent)
    {
    }

    public AIStateID GetId()
    {
        return AIStateID.FREE_FRIENDLY;
    }

    public void Update(Script_Agent agent)
    {
        // Arrive at closest jailed friendly
        agent.Arrive(closestFriendly.transform.position);
        if ((closestFriendly.transform.position - agent.transform.position).magnitude <= 2.0f)
        {
            // free agent
            agent.Manager.enemyJail.Free(closestFriendly);

            // Return to friendly side
            agent.StateMachine.ChangeState(AIStateID.FRIENDLY_RETURN);
        }
    }
}
