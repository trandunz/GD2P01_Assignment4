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

    Transform friendlyHome = null;

    Vector2 Velocity = Vector2.zero;
    Vector2 Acceleration = Vector2.zero;
    Vector2 MousePos = Vector2.zero;

    private void Start()
    {
        friendlyHome = GrabFriendlyHome();
        Manager = transform.root.GetComponent<Script_TeamManager>();
        SetTeamColor();

        StateMachine = new AIStateMachine(this);
        StateMachine.RegisterState(new State_Jailed());
        StateMachine.RegisterState(new State_Idle()) ;
        StateMachine.RegisterState(new State_CaptureFlag());
        StateMachine.RegisterState(new State_FlagReturn());
        StateMachine.ChangeState(initialState);
    }
    void Update()
    {
        GetMousePositon2D();
        Acceleration = Vector2.zero;

        ApplyForce(Vector2.zero);
        StateMachine.Update();

        ApplyAcceleration();
        UpdatePositionBasedOnVelocity();
    }
    Transform GrabFriendlyHome()
    {
        if (RedTeam)
            return transform.root.transform;
        else
            return transform.root.transform;
    }
    void ApplyAcceleration()
    {
        Velocity += Acceleration * Time.deltaTime;
    }
    void UpdatePositionBasedOnVelocity()
    {
        transform.position += new Vector3(Velocity.x, Velocity.y, 0) * Time.deltaTime;
    }
    void SetTeamColor()
    {
        if (RedTeam)
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
        else
            GetComponentInChildren<SpriteRenderer>().color = Color.blue;
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
    public bool IsRedTeam()
    {
        return RedTeam;
    }
    public void SetRedTeam(bool _redTeam)
    {
        RedTeam = _redTeam;
    }
    public Vector2 Seek(Vector2 _position)
    {
        Vector2 desiredVelocity = (_position - GetPosition()).normalized * MaxSpeed;
        Vector2 steeringForce = desiredVelocity - Velocity;
        //steeringForce = Truncate(steeringForce, MaxForce);
        ApplyForce(steeringForce);
        return steeringForce;
    }
    public Vector2 Flee(Vector2 _position)
    {
        Vector2 desiredVelocity = (GetPosition() - _position).normalized * MaxSpeed;
        Vector2 steeringForce = desiredVelocity - Velocity;
        //steeringForce = Truncate(steeringForce, MaxForce);
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
        //steeringForce = Truncate(steeringForce, MaxForce);
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
