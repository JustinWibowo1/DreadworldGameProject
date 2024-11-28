using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire = false; // Initially, you cannot fire until the first shot delay has passed
    private float timer;
    public float timeBetweenFiring;
    public float firstShotDelay; // The delay before the first shot can be fired
    private bool firstShotFired = false; // Flag to track if the first shot has been fired

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // Initialize timer to -firstShotDelay to start counting towards the first shot delay immediately
        timer = -firstShotDelay;
    }

    // Update is called once per frame
    void Update()
{
    mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0; // Assuming the player is at z = 0

    Vector3 rotation = mousePos - transform.position;

    float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

    transform.rotation = Quaternion.Euler(0, 0, rotZ);

    timer += Time.deltaTime;

    // Check if the first shot has been fired
    if (!firstShotFired)
    {
        if (timer >= 0) // Once the timer is >= 0, the first shot delay has passed
        {
            canFire = true; // Allow firing
        }
    }
    else if (timer >= timeBetweenFiring) // For subsequent shots, use timeBetweenFiring
    {
        canFire = true;
    }

    if (Input.GetMouseButtonDown(0) && canFire)
    {
        canFire = false;
        Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        if (!firstShotFired)
        {
            firstShotFired = true; // Mark the first shot as fired
        }
        // Reset timer after firing. For the first shot, it starts counting towards timeBetweenFiring.
        timer = 0;
    }
}
}