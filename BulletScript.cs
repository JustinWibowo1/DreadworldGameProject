using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    public int damageAmount = 10; // Adjust the damage amount as needed
    public float destroyDelay = 2.0f; // Delay before destroying the bullet

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);

        Destroy(gameObject, destroyDelay); // Destroy the bullet after the specified delay
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SolidObjects"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);

            ZombieBehaviour zombie = other.gameObject.GetComponent<ZombieBehaviour>();
            if (zombie != null)
            {
                zombie.TakeDamage(damageAmount);
            }

            FatZombieController FatZombie = other.gameObject.GetComponent<FatZombieController>();
            if (FatZombie != null)
            {
                FatZombie.TakeDamage(damageAmount);
            }

        }
    }
}