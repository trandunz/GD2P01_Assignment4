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
        if ((agent.Manager.transform.position - agent.transform.position).magnitude <= 0.1f)
        {
            agent.Manager.oneOnWayToFlag = false;
            agent.AttachedFlag.Attach(agent.Manager.GetFriendlyFlagHolder());
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
    }
}
