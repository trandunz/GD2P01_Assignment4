using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TeamManager : MonoBehaviour
{
    [SerializeField] bool RedTeam = false;
    Script_Agent[] team;
    Script_Jail enemyJail;
    Script_Flag enemyFlag;

    private void Start()
    {
        team = GetComponentsInChildren<Script_Agent>();
        enemyJail = GrabEnemyJail();
        enemyFlag = GrabEnemyFlag();
    }
    private void Update()
    {
        if(team != null)
        {
            if (enemyFlag == null)
                enemyFlag = GrabEnemyFlag();
            if (enemyFlag != null)
                if (enemyFlag.IsAttachedToAgent())
                    enemyFlag = GrabEnemyFlag();

            if (IsMemberInJail())
                GetClosesMemberToJail()?.SetState(Script_Agent.STATE.SEEK_JAIL);

            if (enemyFlag != null)
            {
                foreach(Script_Agent agent in team)
                {
                    if (!agent.HasFlagAttached())
                        agent.SetFlag(enemyFlag.transform);
                }
                Script_Agent closestToFlag = GetClosesMemberToFlag();
                closestToFlag?.SetState(Script_Agent.STATE.SEEK_FLAG);
            }
        }
    }
    Script_Jail GrabEnemyJail()
    {
        foreach (Script_Jail jail in FindObjectsOfType<Script_Jail>())
        {
            if (jail.IsRedTeam() != RedTeam)
            {
                return jail;
            }
        }
        return null;
    }
    Script_Flag GrabEnemyFlag()
    {
        foreach (Script_Flag flag in FindObjectsOfType<Script_Flag>())
        {
            if (flag.IsRedTeam() != RedTeam && !flag.IsAttachedToAgent())
            {
                return flag;
            }
        }
        return null;
    }
    
    bool IsMemberInJail()
    {
        foreach (Script_Agent agent in team)
        {
            if (agent.GetState() == Script_Agent.STATE.JAILED)
                return true;
        }
        return false;
    }
    Script_Agent GetClosesMemberToJail()
    {
        Script_Agent closest = team[0];
        foreach (Script_Agent agent in team)
        {
            if (agent.GetState() != Script_Agent.STATE.SEEK_HOME
                && agent.GetState() != Script_Agent.STATE.SEEK_JAIL
                && agent.GetState() != Script_Agent.STATE.SEEK_FLAG
                && agent.GetState() != Script_Agent.STATE.SEEK_FRIENDLY_FLAG)
                closest = agent;
            else if (agent.GetState() == Script_Agent.STATE.SEEK_JAIL)
            {
                return null;
            }
        }
        foreach (Script_Agent agent in team)
        {
            if ((enemyJail.transform.position - agent.transform.position).magnitude <= (enemyJail.transform.position - closest.transform.position).magnitude
                && agent.GetState() != Script_Agent.STATE.SEEK_HOME
                && agent.GetState() != Script_Agent.STATE.SEEK_JAIL
                && agent.GetState() != Script_Agent.STATE.SEEK_FLAG
                && agent.GetState() != Script_Agent.STATE.SEEK_FRIENDLY_FLAG)
            {
                closest = agent;
            }
        }
        return closest;
    }
    Script_Agent GetClosesMemberToFlag()
    {
        Script_Agent closest = team[0];
        foreach (Script_Agent agent in team)
        {
            if (agent.GetState() != Script_Agent.STATE.SEEK_HOME
                && agent.GetState() != Script_Agent.STATE.SEEK_JAIL
                && agent.GetState() != Script_Agent.STATE.SEEK_FLAG
                && agent.GetState() != Script_Agent.STATE.SEEK_FRIENDLY_FLAG)
                closest = agent;
            else if(agent.GetState() == Script_Agent.STATE.SEEK_FLAG)
            {
                return null;
            }

        }
        foreach (Script_Agent agent in team)
        {
            if ((enemyFlag.transform.position - agent.transform.position).magnitude <= (enemyFlag.transform.position - closest.transform.position).magnitude
                && agent.GetState() != Script_Agent.STATE.SEEK_HOME
                && agent.GetState() != Script_Agent.STATE.SEEK_JAIL
                && agent.GetState() != Script_Agent.STATE.SEEK_FLAG
                && agent.GetState() != Script_Agent.STATE.SEEK_FRIENDLY_FLAG)
            {
                closest = agent;
            }
        }
        return closest;
    }
    public void SetFlag(Script_Flag _flag)
    {
        enemyFlag = _flag;
    }
}
