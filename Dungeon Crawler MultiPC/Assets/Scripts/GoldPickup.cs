using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int goldAmount = 10; // the amount of gold gained from picking one gold up

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();

            if (playerCombat != null)
            {
                playerCombat.PickupGold(goldAmount);
                Destroy(gameObject); // destroys the gold object
            }
        }
    }
}
