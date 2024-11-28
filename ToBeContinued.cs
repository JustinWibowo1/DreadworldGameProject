using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro
using UnityEngine.SceneManagement; // Import Scene Management

public class ToBeContinued : MonoBehaviour
{
    public string displayText = "Your Text Here"; // Text to display
    public TextMeshProUGUI textComponent; // Assign this in the editor
    public float typingSpeed = 0.05f; // Speed of typing
    public string nextSceneName = "NextScene"; // Name of the next scene to load

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in displayText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(10); // Wait for 10 seconds
        SceneManager.LoadScene(nextSceneName); // Change scene
    }
}