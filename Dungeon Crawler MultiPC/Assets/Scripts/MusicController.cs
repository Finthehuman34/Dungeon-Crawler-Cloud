using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public float BattleMusicRange = 6f; // the range in which the player will be within for the battle music to activate/deactivate
    public AudioSource BackgroundMusic; 
    public AudioSource BattleMusic;

    void Update()
    {
        CheckEnemyProximity(); // check in every frame wether the enemy is within range of the player
    }

    private void CheckEnemyProximity() // checks if the enemy is within the music range of the player
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // identifies the enemy objects in the game and creates a list of them
        bool isPlayerNearEnemy = false; // by defaul there is no enemy within range

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position); // calculates the distance of each enemy to the player
            if (distanceToEnemy <= BattleMusicRange) // if any of the enemies are within range of the player then the battle music is activated
            {
                isPlayerNearEnemy = true;
                break; // needs to exit the loop as player is near enemy is now set true, and needs to be checked again
            }
        }

        
        if (isPlayerNearEnemy)
        {
            if (!BattleMusic.isPlaying)
            {
                // pauses baground music if battle music playing
                BackgroundMusic.Pause();
                BattleMusic.Play();
            }
        }
        else
        {
            if (BattleMusic.isPlaying)
            {
                
                BattleMusic.Pause(); // vice versa
                BackgroundMusic.UnPause();
            }
        }
    }
}
