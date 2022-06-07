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
        agent.Manager.SendAgentToRescue();
    }
    public void Update(Script_Agent agent)
    {
    }
    public void Exit(Script_Agent agent)
    {

    }
}
