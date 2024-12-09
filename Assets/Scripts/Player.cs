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
    public Animator lowerBodyAnimator;
    public Animator upperBodyAnimator;
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

        lowerBodyAnimator.SetFloat("Movement_X", movement.x);
        lowerBodyAnimator.SetFloat("Movement_Y", movement.y);
        lowerBodyAnimator.SetFloat("Speed", movement.sqrMagnitude);
        lowerBodyAnimator.SetFloat("Last_X", lastDirection.x);
        lowerBodyAnimator.SetFloat("Last_Y", lastDirection.y);

        upperBodyAnimator.SetFloat("Movement_X", movement.x);
        upperBodyAnimator.SetFloat("Movement_Y", movement.y);
        upperBodyAnimator.SetFloat("Speed", movement.sqrMagnitude);
        upperBodyAnimator.SetFloat("Last_X", lastDirection.x);
        upperBodyAnimator.SetFloat("Last_Y", lastDirection.y);
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

            lowerBodyAnimator.SetFloat("Attack_X", attackX);
            lowerBodyAnimator.SetFloat("Last_X", lastDirection.x);
            lowerBodyAnimator.SetFloat("Last_Y", lastDirection.y);
            lowerBodyAnimator.SetTrigger("Attack");

            upperBodyAnimator.SetFloat("Attack_X", attackX);
            upperBodyAnimator.SetFloat("Last_X", lastDirection.x);
            upperBodyAnimator.SetFloat("Last_Y", lastDirection.y);
            upperBodyAnimator.SetTrigger("Attack");

            StartCoroutine(EndAttack());


        }

        IEnumerator EndAttack()
        {
            AnimatorStateInfo stateInfo = upperBodyAnimator.GetCurrentAnimatorStateInfo(0);

            while (!stateInfo.IsName("Attack"))
            {
                yield return null;
                stateInfo = upperBodyAnimator.GetCurrentAnimatorStateInfo(0);
            }
            yield return new WaitForSeconds(stateInfo.length);

            isAttacking = false;




        }
    }

}
