using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossZombie : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject fatZombiePrefab;
    [SerializeField] private int spawnAmount = 10;  // Used to control the number of zombies spawned
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private BossHealthScript bossHealthScript;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float maxForce = 10f;
    [SerializeField] private float chaseDistance = 10f;

    private Animator animator;
    private Rigidbody2D rb;
    private int currentHealth;
    private GameObject player;
    private PlayerController playerController;

    // Public booleans to control animation states from the Unity Editor
    public bool isIdle;
    public bool isAttacking;
    public bool isJumping;
    public bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("Rigidbody2D component missing!");
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        bossHealthScript.SetHealth(currentHealth, maxHealth);
        StartCoroutine(BossBehavior());
        StartCoroutine(Roaring());
    }

    void Update()
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isDead", isDead);

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
        Vector2 desiredVelocity = targetDirection * chaseSpeed;
        rb.velocity = desiredVelocity;

        if (targetDirection.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (targetDirection.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    IEnumerator BossBehavior()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(10);
            SpawnZombies();
        }
    }

    void SpawnZombies()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject zombieType = i % 2 == 0 ? zombiePrefab : fatZombiePrefab;
            Instantiate(zombieType, transform.position, Quaternion.identity);
        }
    }

    IEnumerator Roaring()
    {
        while (currentHealth > 0)
        {
            Debug.Log("Roaring");
            yield return new WaitForSeconds(10);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    IEnumerator Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        isDead = false;
    }
}