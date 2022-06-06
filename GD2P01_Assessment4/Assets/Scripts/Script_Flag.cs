using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Flag : MonoBehaviour
{
    [SerializeField]bool RedTeam = false;
    Script_Agent AttachedAgent = null;
    public bool IsRedTeam()
    {
        return RedTeam;
    }
    public void SetRedTeam(bool _redTeam)
    {
        RedTeam = _redTeam;
    }
    private void Update()
    {
        if (AttachedAgent != null)
        {
            transform.position = AttachedAgent.transform.position;
        }
    }
    public void Attach(Script_Agent _agent)
    {
        AttachedAgent = _agent;
        _agent.AttachedFlag = this;
    }
    public void Attach(Script_FlagHolder _flagHolder)
    {
        RedTeam = _flagHolder.IsRedTeam();
        AttachedAgent.AttachedFlag = null;
        AttachedAgent = null;
        transform.parent = _flagHolder.transform;
        transform.position = _flagHolder.transform.position;
    }
    public void Return()
    {
        if (AttachedAgent != null)
        {
            Attach(AttachedAgent.Manager.enemyManager.GetComponent<Script_TeamManager>().GetFriendlyFlagHolder());
        }
    }
    public bool IsAttachedToAgent()
    {
        return AttachedAgent != null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Agent")
        {
            Script_Agent agent = collision.GetComponent<Script_Agent>();
            if (agent.IsRedTeam() != RedTeam && AttachedAgent == null && agent.AttachedFlag == null)
            {
                Attach(agent);
            }
        }
    }
}
