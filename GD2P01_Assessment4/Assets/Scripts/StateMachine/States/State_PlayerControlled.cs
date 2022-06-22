// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : State_PlayerControlled.cs 
// Description : State for being controlled by the player
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PlayerControlled : AIState
{
    public void Enter(Script_Agent agent)
    {
        // Change colour of controlled agent
        if (agent.IsRedTeam())
            agent.SetColor(Color.magenta);
        else
            agent.SetColor(Color.cyan);
    }

    public void Exit(Script_Agent agent)
    {
        // Change color back to team color
        agent.SetTeamColor();
    }

    public AIStateID GetId()
    {
        return AIStateID.PLAYER_CONTROLLED;
    }

    public void Update(Script_Agent agent)
    {
        // Seek the mouse position
        agent.Seek(agent.GetMousePositon2D());
    }
}
