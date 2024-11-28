using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AudioSource gunshotSound;
    private bool isShooting = false;
    private float gunshotCooldown = 1.0f; // 1 second cooldown
    private float lastGunshotTime = -1.0f; // Time since last gunshot

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (gunshotSound == null)
            gunshotSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0) && Time.time >= lastGunshotTime + gunshotCooldown)
        {
            isShooting = true;
            animator.SetBool("isShooting", isShooting);
            gunshotSound.Play();
            lastGunshotTime = Time.time; // Update the last gunshot time
        }
        else if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
        {
            isShooting = false;
            animator.SetBool("isShooting", isShooting);
        }

        // Flip the sprite based on mouse position relative to the player
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }
}