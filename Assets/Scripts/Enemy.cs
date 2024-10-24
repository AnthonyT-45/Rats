using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float health = 10f;

    public float knockbackCooldown = 3f;

    public bool canMove = true;

    private Rigidbody2D rb;
    private float knockbackCooldownTimer = 0f;


    public SpriteRenderer enemySprite;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (!canMove)
            {
                knockbackCooldownTimer -= Time.deltaTime;
                if (knockbackCooldownTimer <= 0f)
                {
                    canMove = true;
                }
                return;
            }

            if (transform.position.x > target.position.x)
            {
                enemySprite.flipX = false;
            }
            else
            {
                enemySprite.flipX = true;
            }

        }

    }

    void FixedUpdate()
    {
        if (target != null)
        {
            if (canMove)
            {
                Vector2 direction = (target.position - transform.position).normalized;


                rb.AddForce(direction * moveSpeed);


                if (rb.velocity.magnitude > moveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * moveSpeed;
                }
            }

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            health -= 1f;
            Debug.Log(health);
        }

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void MovementCooldown()
    {
        canMove = false;
        knockbackCooldownTimer = knockbackCooldown;

    }
}
