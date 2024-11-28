using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this for TextMeshPro
using UnityEngine.SceneManagement; // Add this for scene management

public class StoryScript : MonoBehaviour
{
    public TextMeshProUGUI textMesh1;
    public TextMeshProUGUI textMesh2;
    public float typingSpeed = 0.05f;
    public string sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        textMesh2.gameObject.SetActive(false); // Hide textMesh2 initially
        StartCoroutine(TypeText(textMesh1, "THE YEAR IS 2020", () => {
            StartCoroutine(HideAndShowNextText());
        }));
    }

    IEnumerator TypeText(TextMeshProUGUI textMesh, string text, System.Action onComplete)
    {
        textMesh.text = "";
        foreach (char letter in text.ToCharArray())
        {
            textMesh.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        onComplete?.Invoke();
    }

    IEnumerator HideAndShowNextText()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        textMesh1.gameObject.SetActive(false);
        textMesh2.gameObject.SetActive(true); // Activate textMesh2 before starting to type
        StartCoroutine(TypeText(textMesh2, "One afternoon, when Tono was sleeping at home, suddenly the town was attacked by a terrible zombie outbreak, nobody know what the cause was. Knowing this, Tono's parents protected their son, but unfortunately, Tono's parents didn't know what the zombie's weaknesses were, and they were killed at the hands of zombies. Tono, who saw this, immediately took a bow and accidentally shot it at the head which immediately killed the zombie. With the determination of revenge and wanting to find out who was behind all this, Tono started his adventure", () => {
            StartCoroutine(WaitAndLoadScene());
        }));
    }

    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(5); // Wait for 5 seconds
        SceneManager.LoadScene(sceneToLoad);
    }
}