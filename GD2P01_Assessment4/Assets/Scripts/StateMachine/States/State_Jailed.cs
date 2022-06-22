// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : State_Jailed.cs 
// Description : AIState for being in jail
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Jailed : AIState
{
    public AIStateID GetId()
    {
        return AIStateID.JAILED;
    }
    public void Enter(Script_Agent agent)
    {
        // Upon entering, send an egent to the rescue
        agent.Manager.SendAgentToRescue();
        agent.AttachedFlag = null;
    }
    public void Update(Script_Agent agent)
    {
    }
    public void Exit(Script_Agent agent)
    {

    }
}
