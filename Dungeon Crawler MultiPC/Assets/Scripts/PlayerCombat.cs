using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public int maxHealth = 100; // health set to 100 initially
    private int currentHealth;

    public int maxArmour = 50; // armour initially set to 50
    private int currentArmour;

    public Slider PlayerHealthSlider; // this identifies the correct health slider
    public Slider PlayerArmourSlider;

    private void Start()
    {
        currentHealth = maxHealth; // initialises the health
        currentArmour = maxArmour; // initialises the armour

        PlayerHealthSlider.maxValue = maxHealth; // sets the max value of the player health
        PlayerHealthSlider.value = currentHealth; // set the initial health of the player on the slider

        PlayerArmourSlider.maxValue = maxArmour;  // sets the max value of the player armour
        PlayerArmourSlider.value = currentArmour; // set the initial armour of the player on the slider
    }

    private void Update()
    {

        Debug.Log("Current Armour:" + currentArmour);
        
        if (currentHealth <= 0)
        {
            // will add the death functionality this is just intial stage though
           
        }
    }

    public void TakeDamage(int damage) // this is so the player can take damage from an enemy
    {

        if (currentArmour > 0)
        {
            int remainingDamage = Mathf.Max(0, damage - currentArmour);
            currentArmour = Mathf.Max(0, currentArmour - damage); // takes the damage dealt off the armour
            damage = remainingDamage;
        }

        // if there is still damage to be dealt it does it to the health
        if (damage > 0)
        {
            currentHealth -= damage;
        }


        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth); // just to check it it functioning as expected 

        PlayerHealthSlider.value = currentHealth; // updates the values of the health and armour after damage has been done
        PlayerArmourSlider.value = currentArmour;
    }
}
