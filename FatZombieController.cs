using System.Collections;
using UnityEngine;

public class FatZombieController : MonoBehaviour
{
    public float chaseDistance = 10.0f;
    public float explosionDistance = 1.5f;
    public GameObject explosionEffect;
    public AudioSource explosionAudioSource;

    private bool hasExploded = false;
    public int health = 100; // Health of the zombie
    public GameObject deathEffect; // Effect on death

    void Update()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

            if (distanceToPlayer <= chaseDistance && !hasExploded)
            {
                ChasePlayer(playerObject);
            }

            if (distanceToPlayer <= explosionDistance && !hasExploded)
            {
                Explode();
            }
        }
        else
        {
            Debug.LogWarning("Player object not found with 'Player' tag.");
        }
    }

    void ChasePlayer(GameObject playerObject)
    {
        // Assuming you have an Animator component attached with a "Walk" animation
        GetComponent<Animator>().SetBool("IsWalking", true);
        transform.position = Vector3.MoveTowards(transform.position, playerObject.transform.position, 3f * Time.deltaTime);
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log("TakeDamage called with amount: " + damageAmount); // Debug statement
        health -= damageAmount;

        if (health <= 0)
        {
            Explode();
        }
    }
    

    void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;

            // Show explosion effect
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Create a temporary GameObject to play the explosion sound
            if (explosionAudioSource.clip != null)
            {
                GameObject tempAudioSource = new GameObject("TempAudioSource");
                AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
                audioSource.clip = explosionAudioSource.clip;
                audioSource.Play();
                Destroy(tempAudioSource, explosionAudioSource.clip.length); // Destroy the temp audio source after the clip finishes
            }
            else
            {
                Debug.LogWarning("Explosion AudioSource is not assigned to FatZombieController.");
            }

            // Damage player if within a certain range
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
                if (distanceToPlayer <= explosionDistance) // Check if player is within explosion range
                {
                    if (playerObject.GetComponent<PlayerController>() != null)
                    {
                        playerObject.GetComponent<PlayerController>().TakeDamage(50); // Adjust damage as needed
                    }
                }
            }

            // Destroy the zombie immediately after explosion
            Destroy(gameObject);
        }
    }
}