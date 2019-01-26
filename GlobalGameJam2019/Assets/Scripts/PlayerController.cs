using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float turnSpeed;

    [SerializeField]
    private Transform carryPointN;

    [SerializeField]
    private Transform carryPointS;

    [SerializeField]
    private Transform carryPointE;

    [SerializeField]
    private Transform carryPointW;

    // Store the carry object that we last collided with
    private GameObject carryObject;

    private bool isCarrying;

    private enum Direction { North, South, East, West };

    private Direction idleDirection;

    #endregion

    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
           Vector2 inputDirection = Vector2.zero;

            if (Input.GetKey(KeyCode.Space) && gameObject.GetComponent<Collider2D>().IsTouching(carryObject.GetComponent<Collider2D>()))
            {
                isCarrying = !isCarrying;
                if (!isCarrying)
                {
                    carryObject.transform.parent = null;
                }
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (isCarrying)
                {
                    carryObject.transform.parent = carryPointN;
                    carryObject.transform.localPosition = Vector2.zero;
                }

                inputDirection += Vector2.up;
                idleDirection = Direction.North;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (isCarrying)
                {
                    carryObject.transform.parent = carryPointW;
                    carryObject.transform.localPosition = Vector2.zero;
                }

                inputDirection += Vector2.left;
                idleDirection = Direction.West;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (isCarrying)
                {
                    carryObject.transform.parent = carryPointE;
                    carryObject.transform.localPosition = Vector2.zero;
                }

                inputDirection += Vector2.right;
                idleDirection = Direction.East;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (isCarrying)
                {
                    carryObject.transform.parent = carryPointS;
                    carryObject.transform.localPosition = Vector2.zero;
                }

                inputDirection += Vector2.down;
                idleDirection = Direction.South;
            }

            inputDirection.Normalize();
            inputDirection *= movementSpeed;

            rb.velocity = Vector2.Lerp(rb.velocity, inputDirection, turnSpeed);


            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            carryObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            carryObject = null;
        }
    }
}
