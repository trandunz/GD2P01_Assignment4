using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : AIState
{
    
    public void Enter(Script_Agent agent)
    {
        agent.Acceleration = Vector2.zero;
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
        if (agent.Manager.StartGame)
        {
            if (agent.AttachedFlag != null)
            {
                agent.StateMachine.ChangeState(AIStateID.FLAG_RETURN);
                return;
            }
            agent.Arrive(agent.StartingPosition);
        }
    }
}
