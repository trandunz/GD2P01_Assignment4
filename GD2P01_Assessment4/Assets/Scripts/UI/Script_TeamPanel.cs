// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_TeamPanel.cs 
// Description : Handles funcionality for displaying team statistics
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_TeamPanel : MonoBehaviour
{
    [SerializeField] Text FlagCount;
    [SerializeField] Text JailedMembers;
    [SerializeField] Text FreeMembers;
    [SerializeField] bool RedTeam = false;
    [SerializeField] Script_EndGamePanel endGamePanel = null;

    Script_TeamManager Manager;

    private void Start()
    {
        // Set manager based on redteam value
        foreach(Script_TeamManager manager in FindObjectsOfType<Script_TeamManager>())
        {
            if (manager.RedTeam == RedTeam)
            {
                Manager = manager;
                break;
            }
        }
    }

    private void Update()
    {
        // Update flag count
        FlagCount.text = "Flag Count: " + Manager.GetFriendlyFlagHolder().GetFlagCount().ToString();
        int numberOfJailed = 0;
        int numberOfFree = 0;
        // Update Jailed & free Count
        foreach(Script_Agent agent in Manager.team)
        {
            if (agent.StateMachine.GetStateID() == AIStateID.JAILED)
            {
                numberOfJailed++;
            }
            else
            {
                numberOfFree++;
            }
        }
        // Set texts
        JailedMembers.text = "Jailed Members: " + numberOfJailed.ToString();
        FreeMembers.text = "Free Members: " + numberOfFree.ToString();

        // If flag count on one of the teams is 0, finish the game and set end game panel active
        if (FlagCount.text == "Flag Count: 0")
        {
            foreach (Script_TeamManager manager in FindObjectsOfType<Script_TeamManager>())
            {
                manager.FinishGame();
            }
            endGamePanel.gameObject.SetActive(true);
        }
    }

}
