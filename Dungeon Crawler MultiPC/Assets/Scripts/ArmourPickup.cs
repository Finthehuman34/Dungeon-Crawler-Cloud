using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmourPickup : MonoBehaviour
{
    public int armourPoints = 20; // the amount of armour points the max and current armour stat will increase by

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) // when the collider of the player and armour collide
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();

            if (playerCombat != null) // checks if the playercombat script is being used first
            {
                playerCombat.PickupArmour(armourPoints); // then uses the pickup armour method to alter the armour in the playercombat script
                Destroy(gameObject); // deletes the armour object so it's now been "used"
            }
        }
    }
}
