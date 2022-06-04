using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : AIState
{
    Transform enterPos;
    public void Enter(Script_Agent agent)
    {
        enterPos = agent.transform;
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
        agent.Arrive(enterPos.position);
    }
}
