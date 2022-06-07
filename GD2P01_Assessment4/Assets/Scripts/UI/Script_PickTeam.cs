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
        managers = FindObjectsOfType<Script_TeamManager>();
    }
    public void StartCountDown(bool _redTeam)
    {
        RedTeam = _redTeam;
        countDown = 4.0f;
        CountdownText.gameObject.SetActive(true);
        StartCoroutine(StartGame());
        foreach(var button in Buttons)
        {
            button.SetActive(false);
        }
        TitleText.SetActive(false);
    }
    private void Update()
    {
        MemberCount.text = memberCount.ToString();
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
        yield return new WaitUntil(() => (countDown <= 0.0f));
        CountdownText.text = "Go!";
        yield return new WaitForSeconds(1);
        Panel.SetActive(false);
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
