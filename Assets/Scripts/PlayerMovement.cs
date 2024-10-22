using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection;

    public bool isAttacking = false;



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
            if (!isAttacking)
            {
                isAttacking = true;

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                bool isLeft = mousePosition.x < transform.position.x;

                animator.SetBool("IsAttackLeft", isLeft);
                animator.SetTrigger("Attack");

                if (isLeft)
                {
                    lastDirection = new Vector2(-1, 0);
                }
                else
                {
                    lastDirection = new Vector2(1, 0);
                }
                animator.SetFloat("Last_X", lastDirection.x);
                animator.SetFloat("Last_Y", lastDirection.y);
            }

            animator.SetFloat("Last_Y", lastDirection.y);

        }
    }

    public void AttackAnimationEnd()
    {
        isAttacking = false;
    }


}
