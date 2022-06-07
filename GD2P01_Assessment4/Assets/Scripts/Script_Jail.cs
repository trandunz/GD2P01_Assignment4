using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Jail : MonoBehaviour
{
    [SerializeField] bool RedTeam = false;
    List<Script_Agent> JailedAgents = new List<Script_Agent> ();

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
    public void Jail(Script_Agent _agent)
    {
        if (_agent.AttachedFlag)
        {
            _agent.AttachedFlag.Return();
        }
        _agent.StateMachine.ChangeState(AIStateID.JAILED);
        _agent.Acceleration = Vector2.zero;
        _agent.Velocity = Vector2.zero;
        _agent.transform.position = transform.position;
        JailedAgents.Add(_agent);
    }
    public void Free(Script_Agent _agent)
    {
        JailedAgents.Remove(_agent);
        _agent.StateMachine.ChangeState(AIStateID.FRIENDLY_RETURN);
    }
}
