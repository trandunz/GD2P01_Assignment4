using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Agent : MonoBehaviour
{
    [SerializeField] float MaxSpeed = 20.0f;
    [SerializeField] float MaxForce = 10.0f;
    [SerializeField] float SlowingRadius = 2.0f;
    [SerializeField] Script_Agent TargetAgent = null;
    [SerializeField] bool IsPlayerControlled = false;

    Vector2 Velocity = Vector2.zero;
    Vector2 Acceleration = Vector2.zero;
    Vector2 MousePos = Vector2.zero;
    
    // Update is called once per frame
    void Update()
    {
        GetMousePositon2D();
        Acceleration = Vector2.zero;

        if (!IsPlayerControlled)
            Pursuit(TargetAgent);
        else
            Seek(MousePos);

        Velocity += Acceleration * Time.deltaTime;
        transform.position += new Vector3(Velocity.x, Velocity.y, 0) * Time.deltaTime;
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

    Vector2 Seek(Vector2 _position)
    {
        Vector2 desiredVelocity = (_position - GetPosition()).normalized * MaxSpeed;
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        ApplyForce(steeringForce);
        return steeringForce;
    }
    Vector2 Flee(Vector2 _position)
    {
        Vector2 desiredVelocity = (GetPosition() - _position).normalized * MaxSpeed;
        Vector2 steeringForce = desiredVelocity - Velocity;
        steeringForce = Truncate(steeringForce, MaxForce);
        ApplyForce(steeringForce);
        return steeringForce;
    }
    Vector2 Arrive(Vector2 _position)
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
    Vector2 Pursuit(Script_Agent _agent)
    {
        float distance = (_agent.GetPosition() - GetPosition()).magnitude;
        Vector2 futurePostion = _agent.GetPosition() + (_agent.Velocity * distance/MaxSpeed);
        return Seek(futurePostion);
    }
    Vector2 Evade(Script_Agent _agent)
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
