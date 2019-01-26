using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    #region Fields
    public Transform[] patrolPoints;
    public Rigidbody2D rb;

    private int patrolIndex;
    public float patrolStoppingDistance;
    public float patrolStopTime;
    private float patrolStoppedAt;

    public float speedCasual;
    public float speedRun;
    public float viewDistance;
    public float viewAngle;
    public float turnSpeed;

    public bool IsSanic = false;

    public Vector2 moveDirection;
    #endregion

    private void Start()
    {
        StartCoroutine(CheckLineOfSight());
        StartCoroutine(Move());
    }

    private IEnumerator CheckLineOfSight()
    {
        yield return new WaitUntil(() => { return PlayerController.Instance != null; });
        PlayerController player = PlayerController.Instance;

        while (true)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < viewDistance)
            {
                Vector2 playerDirection = player.transform.position - transform.position;
                Vector2 viewDirection = this.moveDirection;

                float angle = Vector2.Angle(viewDirection, playerDirection);
                if (angle <= viewAngle)
                {
                    IsSanic = true;
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

    protected virtual void MoveFast()
    {
        //implemented in children classes
    }
}
