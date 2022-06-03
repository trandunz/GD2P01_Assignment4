using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Flag : MonoBehaviour
{
    [SerializeField] bool RedTeam = false;
    bool IsAttached = false;
    Script_Agent AttachedAgent = null;
    public bool IsRedTeam()
    {
        return RedTeam;
    }
    private void Update()
    {
        if (AttachedAgent != null && IsAttached)
        {
            transform.position = AttachedAgent.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Agent" && !IsAttached)
        {
            Script_Agent agent = collision.GetComponent<Script_Agent>();
            if (agent.IsRedTeam() != RedTeam && !agent.HasFlagAttached())
            {
                agent.SetState(Script_Agent.STATE.SEEK_FRIENDLY_FLAG);
                agent.SetHasFlag(true);
                IsAttached = true;
                AttachedAgent = agent;
            }
        }
        else
        {
            if ((AttachedAgent.GetFlagHolder().transform.position - transform.position).magnitude < 10)
            {
                AttachedAgent.SetHasFlag(false);
                transform.SetParent(AttachedAgent.GetFlagHolder().transform);
                transform.root.GetComponent<Script_TeamManager>().SetFlag(null);
                IsAttached = false;
                AttachedAgent.GetFlagHolder().AttachFlag(transform);
                RedTeam = !RedTeam;
                AttachedAgent.SetState(Script_Agent.STATE.SEEK_FLAG);
                AttachedAgent = null;
            }
            
        }
    }
    public bool IsAttachedToAgent()
    {
        return IsAttached;
    }
}
