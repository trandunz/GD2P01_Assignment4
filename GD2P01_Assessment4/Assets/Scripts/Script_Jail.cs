// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_Jail.cs 
// Description : Handles funcionality for the jail
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Jail : MonoBehaviour
{
    [SerializeField] bool RedTeam = false;
    List<Script_Agent> JailedAgents = new List<Script_Agent> ();

    private void Start()
    {
    }
    public bool IsRedTeam()
    {
        return RedTeam;
    }
    public void SetRedTeam(bool _redTeam)
    {
        RedTeam = _redTeam;
    }
    public void Jail(Script_Agent _agent)
    {
        // If there no already jailed
        if (_agent.StateMachine.GetStateID() != AIStateID.JAILED)
        {
            // If they have a flag the return it
            if (_agent.AttachedFlag)
            {
                _agent.AttachedFlag.Return();
            }
            // set state to jailed
            _agent.StateMachine.ChangeState(AIStateID.JAILED);
            // Reset velocity
            _agent.Velocity = Vector2.zero;
            // set agent position to jail
            _agent.transform.position = transform.position;
            // add agent to jailed agents list
            JailedAgents.Add(_agent);
        }
    }
    public void Free(Script_Agent _agent)
    {
        // Remove agent from jailed agents
        JailedAgents.Remove(_agent);
        // Change state to friendly return
        _agent.IsFreeingAgent = true;
        _agent.StateMachine.ChangeState(AIStateID.FRIENDLY_RETURN);
    }
}
