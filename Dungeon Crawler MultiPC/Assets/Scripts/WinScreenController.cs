using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenController : MonoBehaviour
{
    public GameObject winScreenUI;
    

    private void Start()
    {
        HideWinScreen(); // hide the win screen at the start
    }

    public void ShowWinScreen()
    {
        winScreenUI.SetActive(true); 
    }

    public void HideWinScreen()
    {
        winScreenUI.SetActive(false);
    }

    

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false; // quits the game
    }




}
