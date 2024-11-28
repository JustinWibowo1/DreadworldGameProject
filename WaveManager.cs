using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Import the TextMeshPro namespace
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class WaveManager : MonoBehaviour
{
    public GameObject zombiePrefab; // Assign the regular zombie prefab in the inspector
    public GameObject fatZombiePrefab; // Assign the FatZombie prefab in the inspector
    public Transform[] spawnPoints; // Assign spawn points in the inspector
    public int[] zombiesPerWave = {5, 10, 15}; // Number of regular zombies in each wave
    public int[] fatZombiesPerWave = {1, 2, 3}; // Number of FatZombies in each wave
    public int totalWaves = 3; // Total number of waves, customizable in the Unity Editor
    public float delayBetweenWaves = 5.0f; // Delay between waves, customizable in the Unity Editor
    public TextMeshProUGUI waveText; // Public variable to hold the reference to the TextMeshPro UI component
    public string nextSceneName = "NextScene"; // Name of the scene to load after completing all waves

    private int currentWave = 0;
    private List<GameObject> activeZombies = new List<GameObject>(); // Track active zombies
    private bool waveInProgress = false; // Flag to check if a wave is currently in progress

    void Start()
    {
        Debug.Log("Game started, preparing to start the first wave.");
        StartCoroutine(StartWaveAfterDelay(delayBetweenWaves)); // Start the first wave after a delay
    }

    void Update()
    {
        if (AllZombiesDead() && !waveInProgress && currentWave < totalWaves)
        {
            Debug.Log("All zombies dead and no wave in progress, starting next wave.");
            StartCoroutine(StartWaveAfterDelay(delayBetweenWaves)); // Start next wave after a delay
        }
    }

    IEnumerator StartWaveAfterDelay(float delay)
    {
        waveInProgress = true; // Set wave in progress flag to true
        Debug.Log($"Delaying next wave by {delay} seconds.");
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        StartWave();
        waveInProgress = false; // Reset wave in progress flag after starting the wave
    }

    void StartWave()
    {
        if (currentWave < totalWaves && currentWave < zombiesPerWave.Length)
        {
            waveText.text = $"Wave {currentWave + 1} Started"; // Update the TextMeshPro text
            StartCoroutine(FadeText(waveText, 1, 3)); // Fade in and out the text
            Debug.Log($"Starting wave: {currentWave + 1}");
            StartCoroutine(SpawnZombies(zombiesPerWave[currentWave], fatZombiesPerWave[currentWave]));
        }
        else
        {
            Debug.Log("All waves completed or index out of range.");
            StartCoroutine(EndGame());
        }
    }

    IEnumerator SpawnZombies(int zombieCount, int fatZombieCount)
    {
        Debug.Log($"Spawning {zombieCount} regular zombies and {fatZombieCount} FatZombies for wave {currentWave + 1}.");
        for (int i = 0; i < zombieCount; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            activeZombies.Add(zombie); // Add spawned zombie to the list
            yield return new WaitForSeconds(1f); // Wait 1 second between each zombie spawn
        }
        for (int j = 0; j < fatZombieCount; j++)
        {
            GameObject fatZombie = Instantiate(fatZombiePrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            activeZombies.Add(fatZombie); // Add spawned FatZombie to the list
            yield return new WaitForSeconds(1f); // Wait 1 second between each FatZombie spawn
        }
        yield return new WaitUntil(() => AllZombiesDead());
        Debug.Log($"Wave {currentWave + 1} completed.");
        currentWave++; // Increment the wave count only after all zombies are confirmed dead

        // Heal the player by 50 health points
        PlayerController playerController = FindObjectOfType<PlayerController>(); // Find the PlayerController in the scene
        if (playerController != null)
        {
            playerController.Heal(50);
            Debug.Log("Player healed by 50 health points after completing the wave.");
        }

        if (currentWave >= totalWaves)
        {
            Debug.Log("All waves completed.");
            StartCoroutine(EndGame());
        }
        else
        {
            Debug.Log("Preparing next wave.");
        }
    }

    IEnumerator EndGame()
    {
        waveText.text = "All Waves Complete"; // Display final message
        StartCoroutine(FadeText(waveText, 1, 3)); // Fade in and out the text
        yield return new WaitForSeconds(5); // Wait for 5 seconds after displaying the message
        SceneManager.LoadScene(nextSceneName); // Load the next scene
    }

    IEnumerator FadeText(TextMeshProUGUI text, float duration, float holdTime)
    {
        text.alpha = 0;
        float time = 0;

        // Fade in
        while (time < duration)
        {
            text.alpha = time / duration;
            time += Time.deltaTime;
            yield return null;
        }
        text.alpha = 1;
        yield return new WaitForSeconds(holdTime);

        // Fade out
        time = 0;
        while (time < duration)
        {
            text.alpha = 1 - (time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        text.alpha = 0;
    }

    bool AllZombiesDead()
    {
        activeZombies.RemoveAll(zombie => zombie == null); // Clean up list to remove null references
        bool allDead = activeZombies.Count == 0;
        return allDead; // Check if list is empty
    }
}