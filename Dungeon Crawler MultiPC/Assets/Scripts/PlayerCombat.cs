using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int maxHealth = 100; // health set to 100 initially
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // initialises the health
    }

    private void Update()
    {
        
        if (currentHealth <= 0)
        {
            // will add the death functionality this is just intial stage though
           
        }
    }

    public void TakeDamage(int damage) // this is so the player can take damage from an enemy
    {
        
        currentHealth -= damage; // reduces the health depending on the damage parameter inputted from the enemy

        
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth); // just to check it it functioning as expected 
    }
}
