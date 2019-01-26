using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public static List<HidingSpot> list = new List<HidingSpot>();

    void Start()
    {
        list.Add(this);
    }
}
