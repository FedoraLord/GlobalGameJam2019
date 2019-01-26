using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    public static PlayerController Instance { get; private set; }

    [SerializeField] private BoxCollider2D box;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform carryPointN;
    [SerializeField] private Transform carryPointS;
    [SerializeField] private Transform carryPointE;
    [SerializeField] private Transform carryPointW;
    
    private bool isCarrying;
    private bool touchingFoxHole;
    private Collider2D carryObject;
    private Direction facing;
    private Transform desiredCarryPoint;
    private Vector3 home;

    private enum Direction
    {
        N, S, W, E
    }

    #endregion

    private void Start()
    {
        Instance = this;
        StartCoroutine(Move());
        home = transform.position;
    }

    private IEnumerator Move()
    {
        while (true)
        {
           Vector2 inputDirection = Vector2.zero;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isCarrying)
                {
                    //drop
                    isCarrying = false;
                    carryObject.transform.parent = null;
                    var food = carryObject.gameObject.GetComponent<FoodController>();
                    if (food.canStore)
                    {
                        carryObject.gameObject.SetActive(false);
                        GameController.Instance.AddScore(food.points);
                        FoodController.list.Remove(food);
                    }
                }
                else if (carryObject != null && box.IsTouching(carryObject))
                {
                    //pick up
                    isCarrying = true;
                }
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                desiredCarryPoint = carryPointN;
                inputDirection += Vector2.up;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                desiredCarryPoint = carryPointS;
                inputDirection += Vector2.down;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                desiredCarryPoint = carryPointW;
                inputDirection += Vector2.left;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                desiredCarryPoint = carryPointE;
                inputDirection += Vector2.right;
            }
            
            inputDirection.Normalize();
            inputDirection *= movementSpeed;

            rb.velocity = Vector2.Lerp(rb.velocity, inputDirection, turnSpeed);

            float xvel = rb.velocity.x;
            float yvel = rb.velocity.y;
            float xabs = Mathf.Abs(xvel);
            float yabs = Mathf.Abs(yvel);

            if (xvel > 0 || yvel > 0)
            {
                if (xabs > yabs)
                {
                    if (xvel > 0)
                    {
                        //right
                        facing = Direction.E;
                        desiredCarryPoint = carryPointE;
                    }
                    else
                    {
                        //left
                        facing = Direction.W;
                        desiredCarryPoint = carryPointW;
                    }
                }
                else
                {
                    if (yvel > 0)
                    {
                        //up
                        facing = Direction.N;
                        desiredCarryPoint = carryPointN;
                    }
                    else
                    {
                        //down
                        facing = Direction.S;
                        desiredCarryPoint = carryPointS;
                    }
                }
            }
            
            //TODO: use "facing" to set animations

            if (isCarrying)
            {
                carryObject.transform.parent = desiredCarryPoint;
                carryObject.transform.localPosition = Vector3.zero;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<FoodController>())
        {
            carryObject = collision;
        }

        if (collision.CompareTag("FoxHole"))
        {
            touchingFoxHole = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FoxHole"))
        {
            touchingFoxHole = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        NPC npc = collision.gameObject.GetComponent<NPC>();
        if (npc != null)
        {
            if (npc is Enemy)
            {
                Die();
            }
            else /*if (npc is Prey)*/
            {
                if (!isCarrying)
                {
                    carryObject = collision.collider;
                }
            }
        }
        
    }

    public void Die()
    {
        //reset all active food and enemy locations
        for (int i = 0; i < Prey.list.Count; i++)
        {
            Prey.list[i].ResetBehavior();
        }

        for (int i = 0; i < Enemy.list.Count; i++)
        {
            Enemy.list[i].ResetBehavior();
        }

        for (int i = 0; i < FoodController.list.Count; i++)
        {
            FoodController.list[i].ResetBehavior();
        }

        //respawn
        transform.position = home;
        rb.velocity = Vector2.zero;
        isCarrying = false;
        carryObject = null;
    }
}
