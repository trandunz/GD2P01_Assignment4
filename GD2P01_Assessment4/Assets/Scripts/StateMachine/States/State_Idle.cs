// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : State_Idle.cs 
// Description : AIState for idling
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : AIState
{
    
    public void Enter(Script_Agent agent)
    {
        // Reset acceelleration and velocity
        agent.Velocity = Vector2.zero;
    }

    public void Exit(Script_Agent agent)
    {
    }

    public AIStateID GetId()
    {
        return AIStateID.IDLE;
    }

    public void Update(Script_Agent agent)
    {
        // If the game is started
        if (agent.Manager.StartGame)
        {
            // If they have a flag, return it
            if (agent.AttachedFlag != null)
            {
                agent.StateMachine.ChangeState(AIStateID.FLAG_RETURN);
                return;
            }
            // else arrive at initial position
            agent.Arrive(agent.StartingPosition);
        }
    }
}
