// Bachelor of Software Engineering 
// Media Design School 
// Auckland 
// New Zealand 
// (c) Media Design School
// File Name : Script_Agent.cs 
// Description : Handles funcionality for all agents in the game
// Author : William Inman
// Mail : william.inman@mds.ac.nz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public bool IsFreeingAgent = false;

    Vector2 SteeringForce = Vector2.zero;
    public Vector2 Velocity = Vector2.zero;
    Vector2 MousePos = Vector2.zero;

    private void Start()
    {
        // Get manager
        Manager = transform.root.GetComponent<Script_TeamManager>();

        // Set colour to team
        SetTeamColor();

        // Register all state machine states
        StateMachine = new AIStateMachine(this);
        StateMachine.RegisterState(new State_Jailed());
        StateMachine.RegisterState(new State_Idle()) ;
        StateMachine.RegisterState(new State_CaptureFlag());
        StateMachine.RegisterState(new State_FlagReturn());
        StateMachine.RegisterState(new State_FreeTeamMate());
        StateMachine.RegisterState(new State_TeamReturn());
        StateMachine.RegisterState(new State_PlayerControlled());
        StateMachine.RegisterState(new State_DefendFromAttack());
        StateMachine.ChangeState(initialState);

        // Set starting position
        StartingPosition = transform.position;
    }
    void Update()
    {
        // Update Mouse Position
        GetMousePositon2D();
        SteeringForce = Vector2.zero;

        // Update State Machine
        StateMachine.Update();

        // if has flag attached
        if (AttachedFlag != null)
        {
            // and is on friendlt side
            if (IsOnFriendySide())
            {
                // Attach flag to friendly flag holder
                AttachedFlag.Attach(Manager.GetFriendlyFlagHolder());
            }
        }

        // If agent is freeing another agent
        if (IsFreeingAgent)
        {
            // If back on friendly side
            if (IsOnFriendySide())
            {
                // isFreeingagent = false
                IsFreeingAgent = false;
            }
        }

        //AvoidOtherAgents();

        // Rotate towards direction of motion
        Vector2 dir = Velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Add steering force to velocity
        Velocity += SteeringForce * Time.deltaTime;

        // Cap velocity to max speed
        Velocity = Truncate(Velocity, MaxSpeed);

        UpdatePositionBasedOnVelocity();
    }

    void UpdatePositionBasedOnVelocity()
    {
        transform.position += new Vector3(Velocity.x, Velocity.y, 0) * Time.deltaTime;
    }
    public void SetTeamColor()
    {
        if (RedTeam)
            SetColor(Color.red);
        else
            SetColor(Color.blue);
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

        ratio = ratio < 1.0f ? ratio : 1.0f;
        _vector *= ratio;
        return _vector;
    }
    Script_Flag GetEnemyFlag()
    {
        // Search through all flags and return enemy one if available
        foreach(Script_Flag flag in FindObjectsOfType<Script_Flag>())
        {
            if (flag.IsRedTeam() != RedTeam && !flag.IsAttachedToAgent())
            {
                return flag;
            }
        }
        return null;
    }
    public Script_Flag GetClosestFlag()
    {
        Script_Flag closestFlag = GetEnemyFlag();

        // compare distances and update closest flag accordingly
        foreach (Script_Flag flag in FindObjectsOfType<Script_Flag>())
        {
            if (flag.IsRedTeam() != RedTeam)
            {
                if ((flag.transform.position - transform.position).magnitude <= (closestFlag.transform.position - transform.position).magnitude
                    && !flag.IsAttachedToAgent())
                {
                    closestFlag = flag;
                }
            }
        }

        return closestFlag;
    }
    public Script_Agent GetClosestJailedAgent()
    {
        // Search through all jailed agents and return enemy one if available
        foreach (Script_Agent agent in Manager.team)
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

        if (!IsVelocityOnFriendlySide() && StateMachine.GetStateID() != AIStateID.JAILED && StateMachine.GetStateID() != AIStateID.PLAYER_CONTROLLED)
        {
            Script_Agent nearestEnemy = Manager.enemyManager.team[0];
            foreach (Script_Agent agent in Manager.enemyManager.team)
            {
                if ((nearestEnemy.transform.position - transform.position).magnitude >= (agent.transform.position - transform.position).magnitude)
                {
                    nearestEnemy = agent;
                }
            }
            float distance = (nearestEnemy.transform.position - transform.position).magnitude;
            float dot = Vector2.Dot(Velocity.normalized, (nearestEnemy.GetPosition() - GetPosition()).normalized);
            Debug.Log("Dot = " + dot.ToString());
            if (distance < 5)
            {
                if (dot > 0)
                {
                    if (dot > 0.5f)
                    {
                        desiredVelocity = (Quaternion.Euler(0, 0, 90) * Velocity).normalized * MaxSpeed;
                    }
                    else
                    {
                        desiredVelocity = (Quaternion.Euler(0, 0, -90) * Velocity).normalized * MaxSpeed;
                    }
                }
                else
                {
                    desiredVelocity = (transform.position - nearestEnemy.transform.position).normalized * MaxSpeed;
                }
            }
            if (desiredVelocity != Vector2.zero)
            {
                steeringForce = desiredVelocity - Velocity;

                SteeringForce = Truncate(steeringForce, MaxForce);
            }
        }
        return steeringForce;
    }
    public Vector2 Seek(Vector2 _position)
    {
        // Get desired velocity
        Vector2 desiredVelocity = (_position - GetPosition()).normalized * MaxSpeed;
        // Apply it to steering force
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        SteeringForce += steeringForce;
        return steeringForce;
    }
    public Vector2 Flee(Vector2 _position)
    {
        // Get desired velocity
        Vector2 desiredVelocity = (GetPosition() - _position).normalized * MaxSpeed;
        // Apply it to steering force
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        SteeringForce += steeringForce;
        return steeringForce;
    }
    public Vector2 Arrive(Vector2 _position)
    {
        // Get distance vector to location from agent
        Vector2 desiredVelocity = (_position - GetPosition());
        float distance = desiredVelocity.magnitude;

        // ramp desired velocity based on distance / slowing radius
        if (distance < SlowingRadius && SlowingRadius > 0)
        {
            desiredVelocity = desiredVelocity.normalized * MaxSpeed * (distance / SlowingRadius);
        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * MaxSpeed;
        }

        // Apply it to steering force
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        SteeringForce += steeringForce;

        return steeringForce;
    }
    public Vector2 Pursuit(Script_Agent _agent)
    {
        // Get future position and seek to it (timeToArrive = d/v)
        float distance = (_agent.GetPosition() - GetPosition()).magnitude;
        Vector2 futurePostion = _agent.GetPosition() + (_agent.Velocity * distance/MaxSpeed);
        SteeringForce += Truncate(Seek(futurePostion), MaxForce);
        return SteeringForce;
    }
    public Vector2 Evade(Script_Agent _agent)
    {
        // Get future position and flee it (timeToArrive = d/v)
        float distance = (_agent.GetPosition() - GetPosition()).magnitude;
        Vector2 futurePostion = _agent.GetPosition() + (_agent.Velocity * distance / MaxSpeed);
        SteeringForce += Truncate(Flee(futurePostion), MaxForce);
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
    public bool IsVelocityOnFriendlySide()
    {
        if (RedTeam && (GetPosition() + Velocity).x < 0)
        {
            return true;
        }
        else if (!RedTeam && (GetPosition() + Velocity).x > 0)
        {
            return true;
        }
        return false;
    }
    public Vector2 GetMousePositon2D()
    {
        MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        return MousePos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If collision with agent and on enemy team and on friendly side then jail him
        if (collision.transform.tag == "Agent")
        {
            Script_Agent otherAgent = collision.transform.GetComponent<Script_Agent>();
            if (otherAgent.IsRedTeam() != RedTeam && IsOnFriendySide())
            {
                Manager.friendlyJail.Jail(otherAgent);

                if(StateMachine.GetStateID() == AIStateID.DEFENDFROMATTACK)
                {
                    StateMachine.ChangeState(AIStateID.IDLE);
                }
            }
            else if (otherAgent.IsRedTeam() == RedTeam && otherAgent.StateMachine.GetStateID() == AIStateID.JAILED && !IsFreeingAgent && StateMachine.GetStateID() == AIStateID.PLAYER_CONTROLLED)
            {
                // free agent
                otherAgent.Manager.enemyJail.Free(otherAgent);

                // Return to friendly side
                otherAgent.StateMachine.ChangeState(AIStateID.FRIENDLY_RETURN);

                IsFreeingAgent = true;
            }
        }
        // If collision with agent and no attached flag then attach flag and set statee to flag return
        else if (collision.transform.tag == "Flag")
        {
            Script_Flag flag = collision.transform.GetComponent<Script_Flag>();
            if (flag.IsRedTeam() != RedTeam && AttachedFlag == null && !IsFreeingAgent)
            {
                flag.Attach(this);

                if (StateMachine.GetStateID() != AIStateID.PLAYER_CONTROLLED)
                    StateMachine.ChangeState(AIStateID.FLAG_RETURN);
            }
        }
    }
}
