using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float health = 10f;

    public SpriteRenderer enemySprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(((direction * moveSpeed) * Time.deltaTime));

        Debug.Log(target.position);

        if (transform.position.x > target.position.x)
        {
            enemySprite.flipX = false;
        }
        else
        {
            enemySprite.flipX = true;
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
}
