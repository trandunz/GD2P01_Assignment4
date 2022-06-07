using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Agent : MonoBehaviour
{
    [SerializeField] float MaxSpeed = 20.0f;
    [SerializeField] float MaxForce = 10.0f;
    [SerializeField] float SlowingRadius = 2.0f;
    [SerializeField] bool IsPlayerControlled = false;
    [SerializeField] float NeighborhoodRange = 10.0f;
    [SerializeField] bool RedTeam = false;

    public AIStateMachine StateMachine;
    public AIStateID initialState;
    public Script_TeamManager Manager = null;

    public Script_Flag AttachedFlag = null;

    public Vector3 StartingPosition;

    bool IsFreeingAgent = false;

    Vector2 SteeringForce = Vector2.zero;
    public Vector2 Velocity = Vector2.zero;
    public Vector2 Acceleration = Vector2.zero;
    Vector2 MousePos = Vector2.zero;

    private void Start()
    {
        Manager = transform.root.GetComponent<Script_TeamManager>();
        SetTeamColor();

        StateMachine = new AIStateMachine(this);
        StateMachine.RegisterState(new State_Jailed());
        StateMachine.RegisterState(new State_Idle()) ;
        StateMachine.RegisterState(new State_CaptureFlag());
        StateMachine.RegisterState(new State_FlagReturn());
        StateMachine.RegisterState(new State_FreeTeamMate());
        StateMachine.RegisterState(new State_TeamReturn());
        StateMachine.RegisterState(new State_PlayerControlled());
        StateMachine.ChangeState(initialState);
        StartingPosition = transform.position;
    }
    void Update()
    {
        GetMousePositon2D();
        Acceleration = Vector2.zero;
        SteeringForce = Vector2.zero;

        StateMachine.Update();

        if (AttachedFlag != null)
        {
            if (RedTeam && transform.position.x < 0)
            {
                AttachedFlag.Attach(Manager.GetFriendlyFlagHolder());
            }
            else if (!RedTeam && transform.position.x > 0)
            {
                AttachedFlag.Attach(Manager.GetFriendlyFlagHolder());
            }
        }
        if (IsFreeingAgent)
        {
            if (RedTeam && transform.position.x < 0)
            {
                IsFreeingAgent = false;
            }
            else if (!RedTeam && transform.position.x > 0)
            {
                IsFreeingAgent = false;
            }
        }

        Vector2 dir = Velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Velocity += SteeringForce * Time.deltaTime;

        UpdatePositionBasedOnVelocity();
    }

    void ApplyAcceleration()
    {
        
    }
    void UpdatePositionBasedOnVelocity()
    {
        transform.position += new Vector3(Velocity.x, Velocity.y, 0) * Time.deltaTime;
    }
    public void SetTeamColor()
    {
        if (RedTeam)
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
        else
            GetComponentInChildren<SpriteRenderer>().color = Color.blue;
    }

    public void SetColor(Color _newcolor)
    {
        GetComponentInChildren<SpriteRenderer>().color = _newcolor;
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
    Script_Flag GetEnemyFlag()
    {
        foreach(Script_Flag flag in FindObjectsOfType<Script_Flag>())
        {
            if (flag.IsRedTeam() != RedTeam)
            {
                return flag;
            }
        }
        return null;
    }
    public Script_Flag GetClosestFlag()
    {
        Script_Flag closestFlag = null;
        foreach (Script_Flag flag in FindObjectsOfType<Script_Flag>())
        {
            if (flag.IsRedTeam() != RedTeam)
            {
                closestFlag = flag;
            }
        }
        foreach (Script_Flag flag in FindObjectsOfType<Script_Flag>())
        {
            if (flag.IsRedTeam() != RedTeam)
            {
                if ((flag.transform.position - transform.position).magnitude <= (closestFlag.transform.position - transform.position).magnitude)
                {
                    closestFlag = flag;
                }
            }
        }
        return closestFlag;
    }
    public Script_Agent GetClosestJailedAgent()
    {
        foreach(Script_Agent agent in Manager.team)
        {
            if (agent.StateMachine.GetStateID() == AIStateID.JAILED)
            {
                return agent;
            }
        }
        return null;
    }
    public bool IsRedTeam()
    {
        return RedTeam;
    }
    public void SetRedTeam(bool _redTeam)
    {
        RedTeam = _redTeam;
    }

    public Vector2 AvoidOtherAgents()
    {
        Vector2 steeringForce = Vector2.zero;
        Vector2 desiredVelocity = Vector2.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right, transform.right, 5);
        if (hit)
        {
            desiredVelocity = (transform.position - new Vector3(hit.point.x, hit.point.y, 0)) * MaxSpeed;
            steeringForce = desiredVelocity - Velocity;
            SteeringForce += steeringForce;
        }
        else
        {
            hit = Physics2D.Raycast((transform.position + transform.right) + transform.up, transform.right, 5);
            if (hit)
            {
                desiredVelocity = (transform.position - new Vector3(hit.point.x, hit.point.y, 0)) * MaxSpeed;
                steeringForce = desiredVelocity - Velocity;
                SteeringForce += steeringForce;
            }
            else
            {
                hit = Physics2D.Raycast((transform.position + transform.right )- transform.up , transform.right, 5);
                if (hit)
                {
                    desiredVelocity = (transform.position - new Vector3(hit.point.x, hit.point.y, 0)) * MaxSpeed;
                    steeringForce = desiredVelocity - Velocity;
                    SteeringForce += steeringForce;
                }
            }
        }

        return steeringForce;
    }
    public Vector2 Seek(Vector2 _position)
    {
        Vector2 desiredVelocity = (_position - GetPosition()).normalized * MaxSpeed;
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        SteeringForce += steeringForce;
        return steeringForce;
    }
    public Vector2 Flee(Vector2 _position)
    {
        Vector2 desiredVelocity = (GetPosition() - _position).normalized * MaxSpeed;
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        SteeringForce += steeringForce;
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
        SteeringForce += steeringForce;

        return steeringForce;
    }
    public Vector2 Pursuit(Script_Agent _agent)
    {
        float distance = (_agent.GetPosition() - GetPosition()).magnitude;
        Vector2 futurePostion = _agent.GetPosition() + (_agent.Velocity * distance/MaxSpeed);
        SteeringForce += Seek(futurePostion);
        return SteeringForce;
    }
    public Vector2 Evade(Script_Agent _agent)
    {
        float distance = (_agent.GetPosition() - GetPosition()).magnitude;
        Vector2 futurePostion = _agent.GetPosition() + (_agent.Velocity * distance / MaxSpeed);
        SteeringForce += Flee(futurePostion);
        return SteeringForce;
    }
    public bool IsOnFriendySide()
    {
        if (RedTeam && transform.position.x < 0)
        {
            return true;
        }
        else if (!RedTeam && transform.position.x > 0)
        {
            return true;
        }
        return false;
    }
    public void ApplyForce(Vector2 _force)
    {
        Acceleration = _force;
    }
    public Vector2 GetMousePositon2D()
    {
        MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        return MousePos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Agent")
        {
            Script_Agent otherAgent = collision.transform.GetComponent<Script_Agent>();
            if (otherAgent.IsRedTeam() != RedTeam && IsOnFriendySide())
            {
                Manager.friendlyJail.Jail(otherAgent);
            }
        }
    }
}
