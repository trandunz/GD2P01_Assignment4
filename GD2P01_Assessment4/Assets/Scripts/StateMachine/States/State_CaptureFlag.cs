// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : State_CaptureFlag.cs 
// Description : AIState for capturing a flag
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_CaptureFlag : AIState
{
    Transform closestFlag = null;
    public void Enter(Script_Agent agent)
    {
        // Get the closet flag , if there isent one, return to idle
        Script_Flag closest = agent.GetClosestFlag();
        if (closest == null)
        {
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
        else
        {
            closestFlag = closest.transform;
        }
    }

    public void Exit(Script_Agent agent)
    {

    }

    public AIStateID GetId()
    {
        return AIStateID.CAPTURE_FLAG;
    }

    public void Update(Script_Agent agent)
    {
        // Arrive at closest flag position
        agent.Arrive(closestFlag.position);
    }
}
