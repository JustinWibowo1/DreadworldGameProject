using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class LoadSceneOnCollision : MonoBehaviour
{
    public string sceneToLoad = "NextSceneName"; // Name of the scene to load

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Goal"))
        {
            SceneManager.LoadScene(sceneToLoad); // Load the specified scene
        }
    }
}