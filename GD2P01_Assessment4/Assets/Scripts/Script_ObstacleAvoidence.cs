using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ObstacleAvoidence : MonoBehaviour
{
    Script_Agent parentAgent;
    private void Start()
    {
        parentAgent = GetComponentInParent<Script_Agent>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Agent")
        {
            Script_Agent agent = collision.GetComponent<Script_Agent>();
            if (agent.IsRedTeam() != parentAgent.IsRedTeam())
            {
                Debug.Log("Avoidence");
                parentAgent.Flee(agent.transform.position);
            }
        }
    }
}
