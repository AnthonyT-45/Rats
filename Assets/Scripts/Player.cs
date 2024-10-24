using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;
    public bool isAttacking = false;
    public float knockbackForce = 20f;
    public float health = 10f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection;




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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody2D rbPlayer = GetComponent<Rigidbody2D>();
            Rigidbody2D rbEnemy = other.GetComponentInParent<Rigidbody2D>();

            Vector2 knockbackDirection = (rbPlayer.position - rbEnemy.position).normalized;

            rbPlayer.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            rbEnemy.AddForce(-knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.MovementCooldown();
            }

            health -= 1f;

            if (health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }


}
