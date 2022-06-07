using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FlagReturn : AIState
{
    void AIState.Enter(Script_Agent agent)
    {
    }

    void AIState.Exit(Script_Agent agent)
    {
    }

    AIStateID AIState.GetId()
    {
        return AIStateID.FLAG_RETURN;
    }

    void AIState.Update(Script_Agent agent)
    {
        agent.Arrive(agent.Manager.transform.position);
        if (agent.IsOnFriendySide())
        {
            agent.Manager.oneOnWayToFlag = false;
            if (agent.AttachedFlag != null)
                agent.AttachedFlag.Attach(agent.Manager.GetFriendlyFlagHolder());
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
    }
}
