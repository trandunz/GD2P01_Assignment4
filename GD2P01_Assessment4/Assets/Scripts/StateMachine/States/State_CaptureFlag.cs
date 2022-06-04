using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_CaptureFlag : AIState
{
    Transform closestFlag = null;
    public void Enter(Script_Agent agent)
    {
        Script_Flag closest = agent.GetClosestFlag();
        if (closest == null)
        {
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
        else
        {
            closestFlag = closest.transform;
        }
    }

    public void Exit(Script_Agent agent)
    {

    }

    public AIStateID GetId()
    {
        return AIStateID.CAPTURE_FLAG;
    }

    public void Update(Script_Agent agent)
    {
        agent.Arrive(closestFlag.position);

        if ((closestFlag.position - agent.transform.position).magnitude <= 0.1f)
        {
            Debug.Log("Grabbged Flag");
            agent.StateMachine.ChangeState(AIStateID.FLAG_RETURN);
        }
    }
}
