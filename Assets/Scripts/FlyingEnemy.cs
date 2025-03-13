using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private int damageAmount = 100;
    [SerializeField] private ParticleSystem explosionParticles;
    private PlayerHealth playerHealth;
    [SerializeField] private float pushForce = 5f;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            PushPlayer(collision);

            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (explosionParticles != null)
        {
            ParticleSystem particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
        }
    }

    private void PushPlayer(Collision2D collision)
    {
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        }
    }
}
