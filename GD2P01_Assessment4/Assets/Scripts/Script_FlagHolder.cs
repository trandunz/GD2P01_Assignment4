using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_FlagHolder : MonoBehaviour
{
    bool RedTeam = false;
    [SerializeField] List<Script_Flag> flags = new List<Script_Flag>();
    private void Start()
    {
        GetComponentInChildren<Script_Flag>().SetRedTeam(RedTeam);
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
        flags.Clear();
        foreach (Script_Flag flag in GetComponentsInChildren<Script_Flag>())
        {
            flags.Add(flag);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Flag")
        {
            Script_Flag flag = collision.GetComponent<Script_Flag>();
        }
    }
}
