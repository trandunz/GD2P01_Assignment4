// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : AIStateMachine.cs 
// Description : Finite state machine for agents to use
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    public AIState[] states;
    public Script_Agent agent;
    public AIStateID currentState;

    /// <summary>
    /// State Machine Contructor
    /// </summary>
    /// <param name="_agent"></param>
    public AIStateMachine(Script_Agent _agent)
    {
        agent = _agent;
        int stateCount = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new AIState[stateCount];
    }
    /// <summary>
    /// Register a new Ai state into list of states
    /// </summary>
    /// <param name="_state"></param>
    public void RegisterState(AIState _state)
    {
        int index = (int)_state.GetId();
        states[index] = _state;
    }
    /// <summary>
    /// Return a state object from its StateID
    /// </summary>
    /// <param name="_stateID"></param>
    /// <returns></returns>
    public AIState GetState(AIStateID _stateID)
    {
        return states[(int)_stateID];
    }
    /// <summary>
    /// Returns current state iD
    /// </summary>
    /// <returns></returns>
    public AIStateID GetStateID()
    {
        return currentState;
    }
    /// <summary>
    /// Calls update of current state every frame
    /// </summary>
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }
    /// <summary>
    /// Change the agents current state to new state
    /// </summary>
    /// <param name="_state"></param>
    public void ChangeState(AIStateID _state)
    {
        GetState(currentState)?.Exit(agent);
        currentState = _state;
        GetState(currentState)?.Enter(agent);
    }
}
