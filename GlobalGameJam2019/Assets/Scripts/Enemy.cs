using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    #region Fields
    public static List<Enemy> list = new List<Enemy>();

    #endregion

    protected override void AddInstance()
    {
        list.Add(this);
    }

    protected override Vector3 GetFastDirection()
    {
        return PlayerController.Instance.transform.position - transform.position;
    }
}
