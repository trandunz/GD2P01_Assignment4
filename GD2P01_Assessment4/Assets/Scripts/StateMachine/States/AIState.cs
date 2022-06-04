using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateID
{
    JAILED,
    IDLE,
    CAPTURE_FLAG,
    FLAG_RETURN
}
public interface AIState
{
    AIStateID GetId();
    void Enter(Script_Agent agent);
    void Update(Script_Agent agent);
    void Exit(Script_Agent agent);
}
