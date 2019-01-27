using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    #region Fields
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sr;

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

    private enum Direction { N, S, W, E };

    private bool isMoving;
    private bool IsSanic = false;
    private Direction facingDir;
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
            Vector3 inputDirection = (IsSanic ? MoveFast() : MoveCasual());

            float xvel = rb.velocity.x;
            float yvel = rb.velocity.y;
            float xabs = Mathf.Abs(xvel);
            float yabs = Mathf.Abs(yvel);
            Direction wasFacing = facingDir;

            if (xabs > 0.1f || yabs > 0.1f)
            {
                if (xabs > yabs)
                {
                    if (xvel > 0.1f)
                    {
                        //right
                        facingDir = Direction.E;
                    }
                    else
                    {
                        //left
                        facingDir = Direction.W;
                    }
                }
                else
                {
                    if (yvel > 0.1f)
                    {
                        //up
                        facingDir = Direction.N;
                    }
                    else
                    {
                        //down
                        facingDir = Direction.S;
                    }
                }
            }

            if (wasFacing != facingDir)
            {
                anim.SetTrigger("DirectionChanged");

                switch (facingDir)
                {
                    case Direction.N:
                        anim.SetInteger("Direction", 0);
                        break;
                    case Direction.S:
                        anim.SetInteger("Direction", 1);
                        break;
                    case Direction.W:
                    case Direction.E:
                        anim.SetInteger("Direction", 2);
                        break;
                }
            }

            sr.flipX = facingDir == Direction.E;

            yield return null;
        }
    }

    protected Vector3 MoveCasual()
    {
        if (patrolPoints.Length == 0)
            return Vector3.zero;

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
        return direction;
    }
    
    private Vector3 MoveFast()
    {
        Vector3 direction = GetFastDirection();
        direction.Normalize();
        direction *= speedRun;
        rb.velocity = Vector2.Lerp(rb.velocity, direction, turnSpeed);
        return direction;
    }

    protected virtual Vector3 GetFastDirection()
    {
        return Vector3.zero;
    }
}
