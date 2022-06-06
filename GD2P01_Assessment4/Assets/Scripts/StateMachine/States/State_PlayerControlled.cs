using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PlayerControlled : AIState
{
    public void Enter(Script_Agent agent)
    {
        if (agent.IsRedTeam())
            agent.SetColor(Color.magenta);
        else
            agent.SetColor(Color.cyan);
    }

    public void Exit(Script_Agent agent)
    {
        agent.SetTeamColor();
    }

    public AIStateID GetId()
    {
        return AIStateID.PLAYER_CONTROLLED;
    }

    public void Update(Script_Agent agent)
    {
        agent.Seek(agent.GetMousePositon2D());
    }
}
