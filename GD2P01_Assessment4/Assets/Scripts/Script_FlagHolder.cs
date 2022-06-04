using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_FlagHolder : MonoBehaviour
{
    bool RedTeam = false;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Flag")
        {
            Script_Flag flag = collision.GetComponent<Script_Flag>();
        }
    }
}
