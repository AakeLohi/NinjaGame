using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private Collider2D collider;

    [SerializeField] private Transform player;

    public float health;
    public float maxHealth;
    public float rotationSpeed;
    public int score;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").transform;    
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            Die();
            return;
        }

        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + rotationSpeed * Time.deltaTime);
        transform.rotation = newRotation;

        if (player != null && transform.position.y <= player.position.y - 30f)
        {
            Destroy(gameObject);
        }
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {


            SlashScript playerScript = other.gameObject.GetComponent<SlashScript>();
            if (playerScript.isSlicing)
            {
                Rigidbody2D playerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
                Vector2 playerVelocity = playerRigidbody.velocity;

                collider.enabled = false;

                Vector3 contactPoint = other.ClosestPoint(transform.position);
                Vector2 otherVelocity = playerVelocity.normalized;
                Quaternion deathEffectRotation = Quaternion.FromToRotation(Vector3.right, otherVelocity);
                Instantiate(deathEffect, contactPoint, deathEffectRotation);

                playerScript.score += score;
                playerScript.slicesThisSlash += 1;
                playerScript.DetectCombo();

                Die();
            }
            else if (playerScript.isFalling)
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity *= -0.5f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Reverse the player's velocity upon collision
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRigidbody.velocity *= -1f;
        }
    }
}
