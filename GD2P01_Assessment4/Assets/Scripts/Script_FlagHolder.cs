// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_FlagHolder.cs 
// Description : Handles funcionality for the flag holder
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_FlagHolder : MonoBehaviour
{
    bool RedTeam = false;
    [SerializeField] List<Script_Flag> flags = new List<Script_Flag>();
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
    public int GetFlagCount()
    {
        return flags.Count;
    }
    private void Update()
    {
        // Grap all flags and set there team correctly
        flags.Clear();
        foreach (Script_Flag flag in GetComponentsInChildren<Script_Flag>())
        {
            flag.SetRedTeam(RedTeam);
            flags.Add(flag);
        }
    }
}
