using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Script_EndGamePanel : MonoBehaviour
{
    [SerializeField] Text Message;
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    private void OnEnable()
    {
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
