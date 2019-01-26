using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public static List<FoodController> list = new List<FoodController>();

    public bool canStore;
    public int points = 1;

    private Vector3 home;

    private void Start()
    {
        list.Add(this);
        home = transform.position;
    }

    public void ResetBehavior()
    {
        transform.position = home;
        transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FoxHole")
        {
            canStore = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FoxHole")
        {
            canStore = false;
        }
    }
}
