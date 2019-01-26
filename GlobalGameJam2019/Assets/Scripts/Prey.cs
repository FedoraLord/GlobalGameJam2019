using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : NPC
{
    #region Fields
    public static List<Prey> list = new List<Prey>();

    private HidingSpot destination;
    #endregion

    protected override void AddInstance()
    {
        list.Add(this);
    }

    public override void ResetBehavior()
    {
        base.ResetBehavior();
        destination = null;
    }

    protected override Vector3 GetFastDirection()
    {
        if (destination == null)
        {
            destination = GetClosestHidingSpot();
        }
        return destination.transform.position - transform.position;
    }

    private HidingSpot GetClosestHidingSpot()
    {
        if (HidingSpot.list.Count == 0)
        {
            Debug.LogError("Add hiding spots");
            return null;
        }

        float minDistance = Mathf.Infinity;
        int bestIndex = 0;

        for (int i = 0; i < HidingSpot.list.Count; i++)
        {
            float dist = Vector3.Distance(HidingSpot.list[i].transform.position, transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                bestIndex = i;
            }
        }

        return HidingSpot.list[bestIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HidingSpot>() != null)
        {
            gameObject.SetActive(false);
            list.Remove(this);
        }
    }
}
