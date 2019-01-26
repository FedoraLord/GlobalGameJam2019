using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : NPC
{
    #region Fields

    #endregion

    protected override void MoveFast()
    {
        Vector2 direction = transform.position - PlayerController.Instance.transform.position;
        Move(direction, speedRun);
    }

    private void Move(Vector2 direction, float speed)
    {
        direction.Normalize();
        direction *= speed;
        rb.velocity = Vector2.Lerp(rb.velocity, direction, turnSpeed);
    }
}
