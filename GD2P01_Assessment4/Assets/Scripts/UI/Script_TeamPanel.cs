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
        FlagCount.text = "Flag Count: " + Manager.GetFriendlyFlagHolder().GetFlagCount().ToString();
        int numberOfJailed = 0;
        int numberOfFree = 0;
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
        JailedMembers.text = "Jailed Members: " + numberOfJailed.ToString();
        FreeMembers.text = "Free Members: " + numberOfFree.ToString();

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
