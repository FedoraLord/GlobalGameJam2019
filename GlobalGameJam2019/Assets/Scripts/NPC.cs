using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    #region Fields
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

    protected virtual void MoveCasual()
    {

    }

    protected virtual void MoveFast()
    {

    }
}
