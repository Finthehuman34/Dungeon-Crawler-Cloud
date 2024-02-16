using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenController : MonoBehaviour
{
    public GameObject deathScreenUI;
    

    private void Start()
    {
        HideDeathScreen(); // hide the death screen at the start
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
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reloads the main game scene
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false; // quits the game
    }
}
