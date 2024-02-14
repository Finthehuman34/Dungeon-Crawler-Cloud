using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int maxArmour = 50;
    private int currentArmour;

    public Slider PlayerHealthSlider;
    public Slider PlayerArmourSlider;

    [SerializeField] private Animator anim;
    [SerializeField] private float meleespeed;
    float timeUntilMelee;
    public GameObject Sword;




    private void Start()
    {
        currentHealth = maxHealth; // initially set the health and armour of the player to their max stats (full health and armour)
        currentArmour = maxArmour;

        PlayerHealthSlider.maxValue = maxHealth; // sets the values of the sliders 
        PlayerHealthSlider.value = currentHealth;
        PlayerArmourSlider.maxValue = maxArmour;
        PlayerArmourSlider.value = currentArmour;

        
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            // will update this
        }

        if(timeUntilMelee <= 0f) // checks if the delay to attack is over
        {
            if (Input.GetMouseButtonDown(0)) // if left mouse button clicked
            {
                anim.SetTrigger("Attack"); // activates the attack animation
                timeUntilMelee = meleespeed; // delay before you can attack again

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get the position of the mouse on the screen
                mousePos.z = 0; // as the game is 2D can ignore z coord

                
                Vector2 direction = (mousePos - (Vector3)Sword.transform.position).normalized; 

                
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // sets the angle at which the sword should swing in relation to the position of the mouse when clicked
                Sword.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));// moves the sword towards that direction
            }
            
        } else
        {
            timeUntilMelee -= Time.deltaTime;
        }

        


    }

    private void OnTriggerEnter2D(Collider2D other) // checks for the collision of the sword and the enemy
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Enemy Hit");
            other.GetComponent<EnemyCombat>().TakeDamage(10); // damages the enemy for 10 points of damage if sword collides with enemy, this can be altered 
        }
    }

    


    public void TakeDamage(int damage)
    {
        if (currentArmour > 0)
        {
            int remainingDamage = Mathf.Max(0, damage - currentArmour);
            currentArmour = Mathf.Max(0, currentArmour - damage);
            damage = remainingDamage;
        }

        if (damage > 0)
        {
            currentHealth -= damage;
        }

        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth + ", Current armour: " + currentArmour);

        PlayerHealthSlider.value = currentHealth;
        PlayerArmourSlider.value = currentArmour;
    }
}
