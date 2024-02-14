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
        currentHealth = maxHealth;
        currentArmour = maxArmour;

        PlayerHealthSlider.maxValue = maxHealth;
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

        if(timeUntilMelee <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("Attack");
                timeUntilMelee = meleespeed;

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0; // Assuming your game is 2D, set the z-coordinate to 0

                // Calculate the direction from the player to the mouse position
                Vector2 direction = (mousePos - (Vector3)Sword.transform.position).normalized;

                // Rotate the sword in the direction of the mouse
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Sword.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            
        } else
        {
            timeUntilMelee -= Time.deltaTime;
        }

        


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Enemy Hit");
            other.GetComponent<EnemyCombat>().TakeDamage(10);
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
