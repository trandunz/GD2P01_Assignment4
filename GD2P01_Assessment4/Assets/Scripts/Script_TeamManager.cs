using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TeamManager : MonoBehaviour
{
    [SerializeField] bool RedTeam = false;
    Script_Agent[] team;
    public Script_Jail enemyJail;
    Script_FlagHolder friendlyflagHolder;
    public bool oneOnWayToFlag = false;

    private void Start()
    {
        enemyJail = GrabEnemyJail();
        team = GetComponentsInChildren<Script_Agent>();
        friendlyflagHolder = GetComponentInChildren<Script_FlagHolder>();
        foreach (Script_Agent agent in team)
        {
            agent.SetRedTeam(RedTeam);
        }
        GetComponentInChildren<Script_Jail>().SetRedTeam(RedTeam);
        GetComponentInChildren<Script_Flag>().SetRedTeam(RedTeam);
        friendlyflagHolder.SetRedTeam(RedTeam);
    }
    private void Update()
    {
        if (team != null)
        {
            
            foreach(Script_Agent agent in team)
            {
                if (agent.StateMachine.GetStateID() != AIStateID.JAILED)
                {
                    if (oneOnWayToFlag == false)
                    {
                        oneOnWayToFlag = true;
                        agent.StateMachine.ChangeState(AIStateID.CAPTURE_FLAG);
                    }
                }
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
    Script_Flag GetClosestFlag()
    {
        foreach(Script_Flag flag in FindObjectsOfType<Script_Flag>())
        {
            if (flag.IsRedTeam() != RedTeam)
            {
                return flag;
            }
        }
        return null;
    }
    public Script_Jail GetEnemyJail()
    {
        return enemyJail;
    }
    public Script_FlagHolder GetFriendlyFlagHolder()
    {
        return friendlyflagHolder;
    }
}
