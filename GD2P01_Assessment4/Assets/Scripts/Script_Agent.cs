using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Agent : MonoBehaviour
{
    public enum STATE
    {
        UNASSIGNED = 0,

        SEEK_FLAG,
        SEEK_FRIENDLY_FLAG,
        SEEK_JAIL,
        SEEK_HOME,

        JAILED,
        DEFENDING_JAIL,
        DEFENDING_FLAG
    }

    [SerializeField] float MaxSpeed = 20.0f;
    [SerializeField] float MaxForce = 10.0f;
    [SerializeField] float SlowingRadius = 2.0f;
    [SerializeField] bool IsPlayerControlled = false;
    [SerializeField] float NeighborhoodRange = 10.0f;

    [SerializeField] bool RedTeam = false;

    bool HasFlag = false;
    [SerializeField]Script_FlagHolder FlagHolder = null;

    Transform friendlyHome = null;
    Transform flagTarget = null;

    STATE Aistate = STATE.UNASSIGNED;
    Script_Agent[] otherAgents = null;
    Script_Jail enemyJail = null;

    Vector2 Velocity = Vector2.zero;
    Vector2 Acceleration = Vector2.zero;
    Vector2 MousePos = Vector2.zero;

    private void Start()
    {
        enemyJail = GrabEnemyJail();
        friendlyHome = GrabFriendlyHome();
        SetTeamColor();
        FlagHolder = transform.root.GetComponentInChildren<Script_FlagHolder>();
        otherAgents = FindObjectsOfType<Script_Agent>();
    }
    void Update()
    {
        GetMousePositon2D();
        Acceleration = Vector2.zero;

        if (IsPlayerControlled)
        { 
            Seek(MousePos);
        }
        else
        {
            HandleStateMachine();
        }

        ApplyAcceleration();
        UpdatePositionBasedOnVelocity();
    }
    Transform GrabFriendlyHome()
    {
        if (RedTeam)
            return GameObject.FindWithTag("RedSide").transform;
        else
            return GameObject.FindWithTag("BlueSide").transform;
    }
    void HandleStateMachine()
    {
        switch (Aistate)
        {
            case STATE.SEEK_JAIL:
                {
                    Seek(enemyJail.transform.position);
                    break;
                }
            case STATE.SEEK_HOME:
                {
                    Seek(friendlyHome.position);
                    break;
                }
            case STATE.SEEK_FLAG:
                {
                    Seek(flagTarget.position);
                    break;
                }
            case STATE.SEEK_FRIENDLY_FLAG:
                {
                    Seek(FlagHolder.transform.position);
                    break;
                }
            default:
                break;
        }
    }
    void ApplyAcceleration()
    {
        Velocity += Acceleration * Time.deltaTime;
    }
    void UpdatePositionBasedOnVelocity()
    {
        transform.position += new Vector3(Velocity.x, Velocity.y, 0) * Time.deltaTime;
    }
    Script_Jail GrabEnemyJail()
    {
        foreach (Script_Jail jail in FindObjectsOfType<Script_Jail>())
        {
            if (jail.IsRedTeam() != RedTeam)
            {
                return jail;
            }
        }
        return null;
    }
    void SetTeamColor()
    {
        if (RedTeam)
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
        else
            GetComponentInChildren<SpriteRenderer>().color = Color.blue;
    }
    Script_Agent IsOtherAgentNear()
    {
        foreach (var agent in otherAgents)
        {
            if ((agent.transform.position - transform.position).magnitude < NeighborhoodRange
                && agent != this)
            {
                return agent;
            }
        }

        return null;
    }

    Vector2 GetPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    Vector2 Truncate(Vector2 _vector, float _maxValue)
    {
        float ratio = 1.0f;
        if (_vector.magnitude > 0)
            ratio = _maxValue/ _vector.magnitude;

        ratio = ratio > 1.0f ? 1.0f : ratio;
        _vector *= ratio;
        return _vector;
    }
    public Script_FlagHolder GetFlagHolder()
    {
        return FlagHolder;
    }
    public void SetHasFlag(bool _trueFalse)
    {
        HasFlag = _trueFalse;
    }
    public bool HasFlagAttached()
    {
        return HasFlag;
    }
    public bool IsRedTeam()
    {
        return RedTeam;
    }
    public void SetFlag(Transform _flag)
    {
        flagTarget = _flag;
    }
    public Vector2 Seek(Vector2 _position)
    {
        Vector2 desiredVelocity = (_position - GetPosition()).normalized * MaxSpeed;
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        ApplyForce(steeringForce);
        return steeringForce;
    }
    public Vector2 Flee(Vector2 _position)
    {
        Vector2 desiredVelocity = (GetPosition() - _position).normalized * MaxSpeed;
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        ApplyForce(steeringForce);
        return steeringForce;
    }
    public Vector2 Arrive(Vector2 _position)
    {
        Vector2 steeringForce = Vector2.zero;
        Vector2 desiredVelocity = (_position - GetPosition());
        float distance = desiredVelocity.magnitude;

        if (distance < SlowingRadius && SlowingRadius > 0)
        {
            desiredVelocity = desiredVelocity.normalized * MaxSpeed * (distance / SlowingRadius);
        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * MaxSpeed;
        }

        steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        ApplyForce(steeringForce);
        return steeringForce;
    }
    public Vector2 Pursuit(Script_Agent _agent)
    {
        float distance = (_agent.GetPosition() - GetPosition()).magnitude;
        Vector2 futurePostion = _agent.GetPosition() + (_agent.Velocity * distance/MaxSpeed);
        return Seek(futurePostion);
    }
    public Vector2 Evade(Script_Agent _agent)
    {
        float distance = (_agent.GetPosition() - GetPosition()).magnitude;
        Vector2 futurePostion = _agent.GetPosition() + (_agent.Velocity * distance / MaxSpeed);
        return Flee(futurePostion);
    }
    public void SetState(STATE _newState)
    {
        Aistate = _newState;
    }
    public STATE GetState()
    {
        return Aistate;
    }

    void ApplyForce(Vector2 _force)
    {
        Acceleration += _force;
    }
    void GetMousePositon2D()
    {
        MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
    }
}
