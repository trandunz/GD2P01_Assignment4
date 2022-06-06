using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TeamManager : MonoBehaviour
{
    public bool RedTeam = false;
    public Script_Agent[] team;
    public Script_Jail enemyJail;
    Script_FlagHolder friendlyflagHolder;
    public Transform enemyManager;
    public bool oneOnWayToFlag = false;
    public bool oneOnWayToJail = false;
    public bool StartGame = false;
    public bool IsPlayersSide = false;
    int PlayerControlledIndex = 0;

    private void Start()
    {
        enemyJail = GrabEnemyJail();
        enemyManager = enemyJail.transform.root;
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
        if (StartGame == true)
        {
            if (IsPlayersSide)
            {
                if (team[PlayerControlledIndex].StateMachine.GetStateID() != AIStateID.PLAYER_CONTROLLED)
                    team[PlayerControlledIndex].StateMachine.ChangeState(AIStateID.PLAYER_CONTROLLED);
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    team[PlayerControlledIndex].StateMachine.ChangeState(AIStateID.IDLE);
                    PlayerControlledIndex = (PlayerControlledIndex + 1) % team.Length;
                    while (team[PlayerControlledIndex].StateMachine.GetStateID() == AIStateID.JAILED)
                    {
                        PlayerControlledIndex = (PlayerControlledIndex + 1) % team.Length;
                    }
                    if (team[PlayerControlledIndex].StateMachine.GetStateID() != AIStateID.PLAYER_CONTROLLED)
                    {
                        team[PlayerControlledIndex].StateMachine.ChangeState(AIStateID.PLAYER_CONTROLLED);
                    }
                }
            }

            if (team != null)
            {
                oneOnWayToFlag = IsFriendlyCapturingFlag();
                foreach (Script_Agent agent in team)
                {
                    if (agent.StateMachine.GetStateID() == AIStateID.IDLE)
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
    }
    public void FinishGame()
    {
        StartGame = false;
        foreach (Script_Agent agent in team)
        {
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
    }
    public void SendAgentToRescue()
    {
        foreach (Script_Agent agent in team)
        {
            if (agent.StateMachine.GetStateID() == AIStateID.IDLE)
            {
                agent.StateMachine.ChangeState(AIStateID.FREE_FRIENDLY);
                Debug.Log(agent.name);
                return;
            }
        }
    }
    bool IsFriendlyCapturingFlag()
    {
        foreach (Script_Agent agent in team)
        {
            if (agent.StateMachine.GetStateID() == AIStateID.CAPTURE_FLAG
                || agent.StateMachine.GetStateID() == AIStateID.FLAG_RETURN)
            {
                return true;
            }
        }
        return false;
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
