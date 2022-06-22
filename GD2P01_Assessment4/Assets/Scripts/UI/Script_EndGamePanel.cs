// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_EndGamePanel.cs 
// Description : Handles game over Ui functionality
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Script_EndGamePanel : MonoBehaviour
{
    [SerializeField] Text Message;

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// Restarts the game scene
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    private void OnEnable()
    {
        // Change win message to whatever team won
        foreach(Script_TeamManager manager in FindObjectsOfType<Script_TeamManager>())
        {
            if (manager.GetFriendlyFlagHolder().GetFlagCount() <= 0)
            {
                if (manager.RedTeam)
                {
                    Message.text = "Blue Team Wins";
                }
                else
                {
                    Message.text = "Red Team Wins";
                }
                break;
            }
        }
        
    }
}
