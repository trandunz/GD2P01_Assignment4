using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TeamManager : MonoBehaviour
{
    [SerializeField] GameObject AgentPrefab;
    public bool RedTeam = false;
    public Script_Agent[] team;
    public Script_Jail enemyJail;
    public Script_Jail friendlyJail;
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
        friendlyJail = GetComponentInChildren<Script_Jail>();
        friendlyflagHolder = GetComponentInChildren<Script_FlagHolder>();

        friendlyflagHolder.SetRedTeam(RedTeam);
        friendlyJail.SetRedTeam(RedTeam);
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
    public void SpawnAgent(int _count)
    {
        int signed = 1;
        if (RedTeam)
        {
            for(int i = 0; i < _count; i++)
            {
                Instantiate(AgentPrefab, new Vector3(-5, i * signed, 0), Quaternion.identity, transform);
                signed *= -1;
            }
        }
        else
        {
            for (int i = 0; i < _count; i++)
            {
                Instantiate(AgentPrefab, new Vector3(5, i * signed, 0), Quaternion.identity, transform);
                signed *= -1;
            }
        }
        team = GetComponentsInChildren<Script_Agent>();
        foreach (Script_Agent agent in team)
        {
            agent.SetRedTeam(RedTeam);
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
