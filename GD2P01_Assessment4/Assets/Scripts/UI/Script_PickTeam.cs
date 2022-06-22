// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_PickTeam.cs 
// Description : Handles team picking UI functionality
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_PickTeam : MonoBehaviour
{
    [SerializeField] Text CountdownText;
    [SerializeField] Text MemberCount;
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject[] Buttons;
    [SerializeField] GameObject TitleText;
    [SerializeField] GameObject OverlayPanel;
    Script_TeamManager[] managers = null;
    float countDown = 0.0f;
    int memberCount = 2;
    bool RedTeam = false;
    private void Start()
    {
        // Get managers
        managers = FindObjectsOfType<Script_TeamManager>();
    }
    public void StartCountDown(bool _redTeam)
    {
        // Assign red team based on player choice
        RedTeam = _redTeam;
        countDown = 4.0f;
        // Enable countdown text
        CountdownText.gameObject.SetActive(true);
        StartCoroutine(StartGame());
        // Set everything disabled
        foreach(var button in Buttons)
        {
            button.SetActive(false);
        }
        TitleText.SetActive(false);
    }
    private void Update()
    {
        // Set member count
        MemberCount.text = memberCount.ToString();
        // Decrease timer by delta time
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
            CountdownText.text = ((int)countDown).ToString();
            
        }
    }
    public void IncreaseMembers()
    {
        if (memberCount < 6)
        {
            memberCount++;

        }
    }
    public void DecreaseMembers()
    {
        if (memberCount > 2)
        {
            memberCount--;
        }
    }
    IEnumerator StartGame()
    {
        // Wait until countdown reaches 0
        yield return new WaitUntil(() => (countDown <= 0.0f));
        // Change countdown text to go
        CountdownText.text = "Go!";
        // Wait a second
        yield return new WaitForSeconds(1);
        // Disable the panel
        Panel.SetActive(false);
        // Set chosen team to players side
        // Set all managers to start game
        // Enable overlay panels
        // Spawn alll agents
        foreach(var manager in managers)
        {
            if (manager.RedTeam == RedTeam)
                manager.IsPlayersSide = true;
            manager.StartGame = true;
            OverlayPanel.SetActive(true);
            manager.SpawnAgent(memberCount);
        }
    }
}
