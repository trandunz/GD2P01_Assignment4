using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FreeTeamMate : AIState
{
    Script_Agent closestFriendly = null;
    public void Enter(Script_Agent agent)
    {
        closestFriendly = agent.GetClosestJailedAgent();
    }

    public void Exit(Script_Agent agent)
    {
    }

    public AIStateID GetId()
    {
        return AIStateID.FREE_FRIENDLY;
    }

    public void Update(Script_Agent agent)
    {
        agent.Arrive(closestFriendly.transform.position);
        if ((closestFriendly.transform.position - agent.transform.position).magnitude <= 1.0f)
        {
            closestFriendly.StateMachine.ChangeState(AIStateID.FRIENDLY_RETURN);
            agent.StateMachine.ChangeState(AIStateID.FRIENDLY_RETURN);
        }
    }
}
