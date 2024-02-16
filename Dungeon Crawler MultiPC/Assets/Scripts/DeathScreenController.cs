using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenController : MonoBehaviour
{
    public GameObject deathScreenUI;
    

    private void Start()
    {
        HideDeathScreen();
    }

    public void ShowDeathScreen()
    {
        deathScreenUI.SetActive(true);
    }

    public void HideDeathScreen()
    {
        deathScreenUI.SetActive(false);
    }

    public void Revive()
    {
        // Implement the logic for reviving the player (to be added later)
        // For now, you can reload the current scene as an example.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
