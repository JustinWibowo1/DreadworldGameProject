using System.Collections;
using UnityEngine;
using TMPro; // For TextMeshPro components

public class StartHomeDialog : MonoBehaviour
{
    public TextMeshProUGUI dialogBox;
    public TextMeshProUGUI nameBox;
    public TextMeshProUGUI guideBox; // TextMeshPro for the guide text
    public AudioSource audioSource;
    public AudioClip typingSound;
    public string[] dialogues;
    public string[] characterNames;
    public string[] guideTexts; // Array of guide texts to display sequentially
    public float typingSpeed = 0.05f;

    private int currentDialogueIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (dialogues.Length > 0 && characterNames.Length > 0)
            StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        while (currentDialogueIndex < dialogues.Length)
        {
            nameBox.text = characterNames[currentDialogueIndex];
            audioSource.clip = typingSound; // Set the clip to play
            audioSource.Play(); // Play the sound at the start of each dialogue
            yield return StartCoroutine(TypeText(dialogues[currentDialogueIndex]));
            currentDialogueIndex++;
            audioSource.Stop(); // Stop the sound when the dialogue is done
            yield return new WaitForSeconds(2); // Wait time between dialogues
        }
        dialogBox.gameObject.SetActive(false); // Deactivate the dialogBox after all dialogues are done
        nameBox.gameObject.SetActive(false); // Deactivate the nameBox after all dialogues are done
        // After the last dialogue, show the guide texts
        StartCoroutine(ShowGuideTexts());
    }

    IEnumerator ShowGuideTexts()
    {
        guideBox.gameObject.SetActive(true); // Make sure the guideBox is active
        foreach (string guide in guideTexts)
        {
            guideBox.text = ""; // Clear previous text if any
            foreach (char letter in guide.ToCharArray())
            {
                guideBox.text += letter;
                yield return new WaitForSeconds(typingSpeed); // Use the same typing speed as dialog
            }
            yield return new WaitForSeconds(5); // Wait for 5 seconds after each guide text
        }
        guideBox.gameObject.SetActive(false); // Hide the guideBox after displaying all texts
    }

    IEnumerator TypeText(string text)
    {
        dialogBox.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}