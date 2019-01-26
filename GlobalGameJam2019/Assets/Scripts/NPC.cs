using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    #region Fields
    public Rigidbody2D rb;

    [Header("Patrolling")]
    public Transform[] patrolPoints;
    public float patrolStoppingDistance;
    public float patrolStopTime;

    [Header("Movement")]
    public float speedCasual;
    public float speedRun;
    public float viewDistance;
    public float viewAngle;
    public float turnSpeed;

    private bool IsSanic = false;
    private float patrolStoppedAt;
    private int patrolIndex;
    private Vector2 facing = Vector2.up;
    private Vector2 home;
    #endregion

    private void Start()
    {
        StartCoroutine(CheckLineOfSight());
        StartCoroutine(Move());
        AddInstance();
        home = transform.position;
    }

    protected virtual void AddInstance()
    {

    }

    public virtual void ResetBehavior()
    {
        IsSanic = false;
        transform.position = home;
        rb.velocity = Vector2.zero;
    }

    private IEnumerator CheckLineOfSight()
    {
        yield return new WaitUntil(() => { return PlayerController.Instance != null; });
        PlayerController player = PlayerController.Instance;

        while (true)
        {
            if (!player.isSneaking)
            {
                if (Vector2.Distance(player.transform.position, transform.position) < viewDistance)
                {
                    Vector2 playerDirection = player.transform.position - transform.position;
                    float angle = Vector2.Angle(facing, playerDirection);
                    
                    if (angle <= viewAngle)
                    {
                        IsSanic = true;
                    }
                }
            }
            yield return null;
        }
    }
    
    private IEnumerator Move()
    {
        while (true)
        {
            if (IsSanic)
            {
                MoveFast();
            }
            else
            {
                MoveCasual();
            }

            yield return null;
        }
    }

    protected void MoveCasual()
    {
        if (patrolPoints.Length == 0)
            return;

        Vector3 direction = Vector3.zero;

        if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) > patrolStoppingDistance)
        {
            //move toward point
            direction = (patrolPoints[patrolIndex].position - transform.position).normalized * speedCasual;
            facing = direction;
        }
        else if (patrolStoppedAt == 0)
        {
            //start slowing to a stop
            patrolStoppedAt = Time.time;
        }
        else if (Time.time > patrolStoppedAt + patrolStopTime)
        {
            //go to next point
            patrolStoppedAt = 0;
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }

        rb.velocity = Vector2.Lerp(rb.velocity, direction, turnSpeed);
    }
    
    private void MoveFast()
    {
        Vector3 direction = GetFastDirection();
        direction.Normalize();
        direction *= speedRun;
        rb.velocity = Vector2.Lerp(rb.velocity, direction, turnSpeed);
    }

    protected virtual Vector3 GetFastDirection()
    {
        return Vector3.zero;
    }
}
