using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    public int maxHealth = 50; // enemy has less health than player
    private int currentHealth;
    public Slider healthSlider;
    public Canvas healthCanvas;

    private void Start()
    {
        currentHealth = maxHealth;

        healthCanvas = Instantiate(Resources.Load("HealthCanvas") as Canvas, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity); // keeps the slider locked to above the enemy's head
        healthSlider = healthCanvas.GetComponentInChildren<Slider>(); // accesses the correct slider for the enemy health
        healthSlider.maxValue = maxHealth; // set the max value of the slider (the most health the enemy can have)
        healthSlider.value = currentHealth;

        
        
    }

    private void Update()
    {
        
        
        
        if (currentHealth <= 0) // if the enemy dies it will be deleted from the game
        {
            
            
            Destroy(gameObject); 
            Destroy(healthCanvas.gameObject);   
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // check whether the collision boxes of the player and enemy have collided
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(10); // uses the TakeDamage method in the player class to cause 10 damage to the player

            
        }
    }

    

    public void TakeDamage(int damage) // this is so the enemy can take damage, same logic as the method in the player class
    {
        // the enemy attack will deal this damage when added
        currentHealth -= damage;
        healthSlider.value = currentHealth;
    }
}
