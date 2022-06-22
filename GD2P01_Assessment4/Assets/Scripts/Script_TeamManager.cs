// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_TeamManager.cs 
// Description : Acts as the manager for a team, changing there states and making decisions
// Author : William Inman
// Mail : william.inman@mds.ac.nz

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
    public Script_TeamManager enemyManager;
    public bool oneOnWayToFlag = false;
    public bool oneOnWayToJail = false;
    public bool StartGame = false;
    public bool IsPlayersSide = false;
    int PlayerControlledIndex = 0;
    bool doOnce = true;

    private void Start()
    {
        // Get Enemy & friendly jail, friendly flag holder, enemy manager
        enemyJail = GrabEnemyJail();
        enemyManager = enemyJail.transform.root.GetComponent<Script_TeamManager>();
        friendlyJail = GetComponentInChildren<Script_Jail>();
        friendlyflagHolder = GetComponentInChildren<Script_FlagHolder>();

        // Set objects team correctly
        friendlyflagHolder.SetRedTeam(RedTeam);
        friendlyJail.SetRedTeam(RedTeam);
    }
    private void Update()
    {
        // If game is on
        if (StartGame == true)
        {
            // If player side
            if (IsPlayersSide)
            {
                // at start set one of the agents to player controlled
                if (doOnce)
                {
                    doOnce = false;
                    if (team[PlayerControlledIndex].StateMachine.GetStateID() != AIStateID.PLAYER_CONTROLLED)
                        team[PlayerControlledIndex].StateMachine.ChangeState(AIStateID.PLAYER_CONTROLLED);
                }
                // if shift pressed
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    // Set old player controlled agent to idle if not jailed
                    if (team[PlayerControlledIndex].StateMachine.GetStateID() != AIStateID.JAILED && team[PlayerControlledIndex].StateMachine.GetStateID() != AIStateID.FRIENDLY_RETURN
                        && team[PlayerControlledIndex].StateMachine.GetStateID() != AIStateID.FLAG_RETURN)
                        team[PlayerControlledIndex].StateMachine.ChangeState(AIStateID.IDLE);

                    // Increment player controlled index
                    PlayerControlledIndex = (PlayerControlledIndex + 1) % team.Length;
                    // incremeber player controlled index until non jailed member is found OR tried too many times
                    int tries = 0;
                    while (team[PlayerControlledIndex].StateMachine.GetStateID() == AIStateID.JAILED || team[PlayerControlledIndex].StateMachine.GetStateID() == AIStateID.FRIENDLY_RETURN
                        || team[PlayerControlledIndex].StateMachine.GetStateID() == AIStateID.FLAG_RETURN)
                    {
                        if (tries == team.Length + 1)
                            break;
                        else
                            tries++;
                        PlayerControlledIndex = (PlayerControlledIndex + 1) % team.Length;
                    }
                    // If agent is not player controlled and not jailed then set player controlled
                    if (team[PlayerControlledIndex].StateMachine.GetStateID() != AIStateID.PLAYER_CONTROLLED
                        && team[PlayerControlledIndex].StateMachine.GetStateID() != AIStateID.JAILED)
                    {
                        team[PlayerControlledIndex].StateMachine.ChangeState(AIStateID.PLAYER_CONTROLLED);
                    }
                }
            }

            if (team != null)
            {
                foreach(var enemyagent in enemyManager.team)
                {
                    if (!enemyagent.IsOnFriendySide() && enemyagent.StateMachine.GetStateID() != AIStateID.JAILED)
                    {
                        foreach (Script_Agent friendlyAgent in team)
                        {
                            if (friendlyAgent.StateMachine.GetStateID() == AIStateID.IDLE)
                            {
                                friendlyAgent.StateMachine.ChangeState(AIStateID.DEFENDFROMATTACK);
                                break;
                            }
                        }
                        break;
                    }
                }

                // set bool if friendly is on way to capture a flag
                oneOnWayToFlag = IsFriendlyCapturingFlag();

                // Loop through all agents in team, if any are idling and no one is on way to flag then send one
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
        // Spawn number of agents on correct side
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

        // add them too team array and set there team correctly
        team = GetComponentsInChildren<Script_Agent>();
        foreach (Script_Agent agent in team)
        {
            agent.SetRedTeam(RedTeam);
        }
    }
    public void FinishGame()
    {
        StartGame = false;
        // Set all agents idle
        foreach (Script_Agent agent in team)
        {
            agent.StateMachine.ChangeState(AIStateID.IDLE);
        }
    }
    public void SendAgentToRescue()
    {
        // Search through all agents, if any are idle then send them to free friendly
        foreach (Script_Agent agent in team)
        {
            if (agent.StateMachine.GetStateID() == AIStateID.IDLE)
            {
                agent.StateMachine.ChangeState(AIStateID.FREE_FRIENDLY);
                return;
            }
        }
    }
    bool IsFriendlyCapturingFlag()
    {
        // Return bool if any agent is capturing flag or on way back
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
        // Seaarch through all jails and get team jail that isent this team
        foreach (Script_Jail jail in FindObjectsOfType<Script_Jail>())
        {
            if (jail.IsRedTeam() != RedTeam)
            {
                return jail;
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
