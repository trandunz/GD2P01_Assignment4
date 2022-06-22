// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_MainMenu.cs 
// Description : Main Menu Controls
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_MainMenu : MonoBehaviour
{
    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
