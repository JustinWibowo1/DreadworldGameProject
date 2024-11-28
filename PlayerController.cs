using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include to handle UI elements

public class PlayerController : MonoBehaviour
{
    public HealthStat healthStat;
    public StaminaStat staminaStat; // Using StaminaStat instead of ExpStat

    public float moveSpeed;
    public float deceleration = 10f;

    private Vector2 input;
    private Vector3 currentVelocity = Vector3.zero;

    private Animator animator;

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    // Add a public reference to the UI Image that will be used for fading
    public Image fadeOutUIImage;

    // Add a boolean to control player movement
    private bool canMove = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Initialize stats
        healthStat = GameObject.Find("HealthBar").GetComponent<HealthStat>();
        staminaStat = GameObject.Find("StaminaBar").GetComponent<StaminaStat>();

        // Set initial values for stats
        healthStat.MyMaxValue = 100;
        healthStat.MyCurrentValue = 100;

        staminaStat.MyMaxValue = 100;
        staminaStat.MyCurrentValue = 100;

        // Ensure the fadeOutUIImage is initially invisible if not set in the editor
        if (fadeOutUIImage != null)
            fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, 0);
    }

    void Update()
    {
        HandleUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collision detected.");
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        healthStat.MyCurrentValue -= damage;
        Debug.Log("Player took damage. Current health: " + healthStat.MyCurrentValue);

        if (healthStat.MyCurrentValue <= 0)
        {
            Debug.Log("Player health reached 0. Starting fade out.");
            StartCoroutine(FadeOutAndReload());
        }
    }

    IEnumerator FadeOutAndReload()
    {
        canMove = false; // Disable player movement
        float fadeSpeed = 1f;
        Color fadeColor = fadeOutUIImage.color;
        while (fadeColor.a < 1)
        {
            fadeColor.a += Time.deltaTime * fadeSpeed;
            fadeOutUIImage.color = fadeColor;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HandleUpdate()
    {
        if (!canMove) return; // Skip update if movement is disabled

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        float currentMoveSpeed = moveSpeed;
        bool canSprint = staminaStat.MyCurrentValue > 10; // Set a threshold for re-enabling sprinting

        // Check if the Shift key is held down to sprint and there is enough stamina
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && canSprint)
        {
            currentMoveSpeed *= 2;  // Increase the move speed by a factor (e.g., 2)
            staminaStat.MyCurrentValue -= Time.deltaTime * 30;  // Increase stamina depletion rate
            if (staminaStat.MyCurrentValue <= 0)
            {
                staminaStat.MyCurrentValue = 0; // Ensure stamina doesn't go below zero
                canSprint = false; // Disable sprinting when stamina is depleted
            }
        }

        if (input != Vector2.zero)
        {
            Vector3 move = new Vector3(input.x, input.y, 0).normalized * currentMoveSpeed * Time.deltaTime;
            MovePlayer(move);
        }
        else
        {
            transform.position += currentVelocity * deceleration * Time.deltaTime;
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mouseWorldPosition - transform.position).normalized;
        animator.SetFloat("aimX", directionToMouse.x);
        animator.SetFloat("aimY", directionToMouse.y);

        animator.SetBool("isMoving", input != Vector2.zero);

        if (Input.GetKeyDown(KeyCode.E))
            Interact();

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("isShooting", true);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    private void MovePlayer(Vector3 move)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, move, move.magnitude, solidObjectsLayer | interactableLayer);
        if (hit.collider == null)
        {
            transform.position += move;
            currentVelocity = move;
        }
        else
        {
            currentVelocity = Vector3.zero;
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        facingDir.Normalize();

        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    public void Heal(int healAmount)
    {
        healthStat.MyCurrentValue += healAmount;
        if (healthStat.MyCurrentValue > healthStat.MyMaxValue)
        {
            healthStat.MyCurrentValue = healthStat.MyMaxValue; // Ensure health does not exceed maximum
        }
        Debug.Log("Player healed. Current health: " + healthStat.MyCurrentValue);
    }
}