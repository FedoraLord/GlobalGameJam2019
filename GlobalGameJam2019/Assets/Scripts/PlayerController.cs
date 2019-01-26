using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    public static PlayerController Instance { get; private set; }

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float turnSpeed;
    #endregion

    private void Start()
    {
        Instance = this;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            Vector2 inputDirection = Vector2.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                inputDirection += Vector2.up;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                inputDirection += Vector2.left;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                inputDirection += Vector2.right;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                inputDirection += Vector2.down;

            inputDirection.Normalize();
            inputDirection *= movementSpeed;

            rb.velocity = Vector2.Lerp(rb.velocity, inputDirection, turnSpeed);

            yield return new WaitForFixedUpdate();
        }
    }
}
