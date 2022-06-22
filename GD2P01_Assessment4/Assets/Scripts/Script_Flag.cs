// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_Flag.cs 
// Description : Handles funcionality for a flag such as attaching to flagHolder and also agents
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Flag : MonoBehaviour
{
    [SerializeField]bool RedTeam = false;
    Script_Agent AttachedAgent = null;
    public bool IsRedTeam()
    {
        return RedTeam;
    }
    public void SetRedTeam(bool _redTeam)
    {
        RedTeam = _redTeam;
    }
    private void Update()
    {
        if (AttachedAgent != null)
        {
            transform.position = AttachedAgent.transform.position;
        }
    }
    public void Attach(Script_Agent _agent)
    {
        // Disable collider
        GetComponent<CircleCollider2D>().enabled = false;
        // Attach to agent
        AttachedAgent = _agent;
        _agent.AttachedFlag = this;
    }
    public void Attach(Script_FlagHolder _flagHolder)
    {
        // Set team accordingly
        RedTeam = _flagHolder.IsRedTeam();
        // reset attached flag and attached agent
        AttachedAgent.AttachedFlag = null;
        AttachedAgent = null;
        // Update parent
        transform.parent = _flagHolder.transform;
        // Set position
        transform.position = _flagHolder.transform.position;
        // Enable collider
        GetComponent<CircleCollider2D>().enabled = true;
    }
    public void Return()
    {
        // If flag is attached then return it to enemy flag holder
        if (AttachedAgent != null)
        {
            Attach(AttachedAgent.Manager.enemyManager.GetComponent<Script_TeamManager>().GetFriendlyFlagHolder());
        }
    }
    public bool IsAttachedToAgent()
    {
        return AttachedAgent != null;
    }
}
