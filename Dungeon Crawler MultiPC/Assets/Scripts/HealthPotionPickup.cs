using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionPickup : MonoBehaviour
{
    public int maxHealthIncrease = 20;  // increase max health by 20
    public int healthRestore = 30; // restore 20 health points

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // checks for the collision with the player
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();

            if (playerCombat != null)
            {
                playerCombat.HealthPickupPotion(maxHealthIncrease, healthRestore); // adjusts the stats within player combat script accordingly
                Destroy(gameObject); // destroys the potion
            }
        }
    }
}
