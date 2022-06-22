using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_DefendFromAttack : AIState
{
    Script_Agent targetAgent;
    public void Enter(Script_Agent agent)
    {
        foreach (var enemyagent in agent.Manager.enemyManager.team)
        {
            if (!enemyagent.IsOnFriendySide() && enemyagent.StateMachine.GetStateID() != AIStateID.JAILED)
            {
                targetAgent = enemyagent;
                break;
            }
        }
        Script_Agent nearestEnemy = targetAgent;
        foreach (var enemyagent in agent.Manager.enemyManager.team)
        {
            if ((nearestEnemy.transform.position - agent.transform.position).magnitude >= (enemyagent.transform.position - agent.transform.position).magnitude
                && enemyagent.StateMachine.GetStateID() != AIStateID.JAILED)
            {
                targetAgent = enemyagent;
            }
        }
    }

    public void Exit(Script_Agent agent)
    {
    }

    public AIStateID GetId()
    {
        return AIStateID.DEFENDFROMATTACK;
    }

    public void Update(Script_Agent agent)
    {
        if (targetAgent == null)
        {
            agent.StateMachine.ChangeState(AIStateID.IDLE);
            return;
        }

        agent.Pursuit(targetAgent);
        if (targetAgent.StateMachine.GetStateID() == AIStateID.JAILED || targetAgent.IsOnFriendySide())
        {
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
    }
}
