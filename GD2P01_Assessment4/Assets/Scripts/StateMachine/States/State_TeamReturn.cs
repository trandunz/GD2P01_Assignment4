using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_TeamReturn : AIState
{
    void AIState.Enter(Script_Agent agent)
    {
    }

    void AIState.Exit(Script_Agent agent)
    {
    }

    AIStateID AIState.GetId()
    {
        return AIStateID.FRIENDLY_RETURN;
    }

    void AIState.Update(Script_Agent agent)
    {
        agent.Arrive(agent.Manager.transform.position);
        if (agent.IsRedTeam() && agent.transform.position.x < 0)
        {
            agent.Manager.oneOnWayToJail = false;
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
        else if (!agent.IsRedTeam() && agent.transform.position.x > 0)
        {
            agent.Manager.oneOnWayToJail = false;
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
    }
}
