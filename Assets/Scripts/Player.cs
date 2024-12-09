using System.Collections;
using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Animator bodyAnimator;
    public Animator armAnimator;
    public float health = 10f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection;

    private float attackX;

    private bool isAttacking = false;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastDirection = new Vector2(0, -1);
    }

    void Update()
    {
        if (!isAttacking)
        {
            Movement();
        }
        Attack();
    }

    void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    void MovementAnimation(Animator animator)
    {
        animator.SetFloat("Movement_X", movement.x);
        animator.SetFloat("Movement_Y", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("Last_X", lastDirection.x);
        animator.SetFloat("Last_Y", lastDirection.y);
    }

    void AttackAnimation(Animator animator)
    {
        animator.SetFloat("Attack_X", attackX);
        animator.SetFloat("Last_X", lastDirection.x);
        animator.SetFloat("Last_Y", lastDirection.y);
        animator.SetTrigger("Attack");
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

        MovementAnimation(bodyAnimator);
        MovementAnimation(armAnimator);
    }

    void Attack()
    {

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            bool isLeft = mousePosition.x < transform.position.x;

            if (isLeft)
            {
                attackX = -1f;
            }
            else
            {
                attackX = 1f;
            }

            lastDirection = new Vector2(attackX, 0f);

            AttackAnimation(armAnimator);

            StartCoroutine(EndAttack());


        }

        IEnumerator EndAttack()
        {
            AnimatorStateInfo stateInfo = armAnimator.GetCurrentAnimatorStateInfo(0);

            while (!stateInfo.IsName("Attack"))
            {
                yield return null;
                stateInfo = armAnimator.GetCurrentAnimatorStateInfo(0);
            }
            yield return new WaitForSeconds(stateInfo.length);

            isAttacking = false;




        }



    }

}
