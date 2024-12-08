using System.Collections;
using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;
    public float health = 10f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection;

    private float attackX;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastDirection = new Vector2(0, -1);
    }

    void Update()
    {
        Movement();
        Attack();
    }

    void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    void Movement()
    {
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement.y += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.y -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x += 1f;
        }

        movement = movement.normalized;

        if (movement != Vector2.zero)
        {
            lastDirection = movement;
        }
        animator.SetFloat("Movement_X", movement.x);
        animator.SetFloat("Movement_Y", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("Last_X", lastDirection.x);
        animator.SetFloat("Last_Y", lastDirection.y);
    }

    void Attack()
    {

        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            bool isLeft = mousePosition.x < transform.position.x;

            if (isLeft)
            {
                attackX = -1f;
            }
            else
            {
                attackX = 1f;
            }

            lastDirection = new Vector2(attackX, 0);
            animator.SetFloat("Attack_X", attackX);
            animator.SetFloat("Last_X", lastDirection.x);
            animator.SetFloat("Last_Y", lastDirection.y);
            animator.SetFloat("Last_X", lastDirection.x);
            animator.SetFloat("Last_Y", lastDirection.y);
            animator.SetTrigger("Attack");




        }
    }

}
