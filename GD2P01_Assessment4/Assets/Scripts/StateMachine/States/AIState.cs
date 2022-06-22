// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : AIState.cs 
// Description : AIState interface for other states
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the possible states an agent could be in
/// </summary>
public enum AIStateID
{
    JAILED,
    IDLE,
    CAPTURE_FLAG,
    FLAG_RETURN,
    FREE_FRIENDLY,
    FRIENDLY_RETURN,
    PLAYER_CONTROLLED,
    DEFENDFROMATTACK
}

public interface AIState
{
    /// <summary>
    /// Returns the current states AIStateID
    /// </summary>
    /// <returns></returns>
    AIStateID GetId();

    /// <summary>
    /// Encapsules implementation for when state is entered
    /// </summary>
    /// <param name="agent"></param>
    void Enter(Script_Agent agent);

    /// <summary>
    /// Encapsules implementation for state update
    /// </summary>
    /// <param name="agent"></param>
    void Update(Script_Agent agent);

    /// <summary>
    /// Encapsules implementation for when leaving the state
    /// </summary>
    /// <param name="agent"></param>
    void Exit(Script_Agent agent);
}
