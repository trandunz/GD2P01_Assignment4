using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    public AIState[] states;
    public Script_Agent agent;
    public AIStateID currentState;

    public AIStateMachine(Script_Agent _agent)
    {
        agent = _agent;
        int stateCount = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new AIState[stateCount];
    }
    public void RegisterState(AIState _state)
    {
        int index = (int)_state.GetId();
        states[index] = _state;
    }
    public AIState GetState(AIStateID _stateID)
    {
        return states[(int)_stateID];
    }
    public AIStateID GetStateID()
    {
        return currentState;
    }
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }
    public void ChangeState(AIStateID _state)
    {
        GetState(currentState)?.Exit(agent);
        currentState = _state;
        GetState(currentState)?.Enter(agent);
    }
}
