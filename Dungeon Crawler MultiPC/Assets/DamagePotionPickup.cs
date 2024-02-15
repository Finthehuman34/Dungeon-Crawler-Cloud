using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePotionPickup : MonoBehaviour
{
    public int damageIncrease = 10; // increase damage dealt by player by 10 damage points

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();

            if (playerCombat != null)
            {
                playerCombat.DamagePotionPickup(damageIncrease); // applies the damage increase within the player combat class
                Destroy(gameObject); // destroy potion object
            }
        }
    }
}
