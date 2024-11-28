using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    [SerializeField]
    private float chaseSpeed = 5f;
    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private AudioClip deathSound; // Audio clip for death sound
    [SerializeField]
    private float maxForce = 10f;
    [SerializeField]
    private float chaseDistance = 10f;

    private Animator animator;
    private Rigidbody2D rb;
    public int health = 100;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("Rigidbody2D component missing!");
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= chaseDistance)
            {
                ChasePlayer(player);
            }
        }
    }

    void ChasePlayer(GameObject player)
    {
        Vector2 targetDirection = (player.transform.position - transform.position).normalized;
        Vector2 currentVelocity = rb.velocity;
        Vector2 desiredVelocity = targetDirection * chaseSpeed;
        Vector2 steering = desiredVelocity - currentVelocity;
        steering = Vector2.ClampMagnitude(steering, maxForce);
        rb.AddForce(steering);

        // Correctly flip the zombie based on the direction of movement without altering its size
        if (targetDirection.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (targetDirection.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Zombie is dying");
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Create a temporary GameObject to play the death sound, starting from 1 second into the audio clip
        if (deathSound != null)
        {
            GameObject audioPlayer = new GameObject("TempAudio");
            AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();
            audioSource.clip = deathSound;
            audioSource.Play();
            audioSource.time = 1f; // Start playback 1 second into the clip
            Destroy(audioPlayer, Mathf.Max(0, deathSound.length - 1f)); // Destroy the audio player after the remaining length of the clip
        }

        Destroy(gameObject);
    }

    private void DestroyZombie()
    {
        Destroy(gameObject);
    }
}