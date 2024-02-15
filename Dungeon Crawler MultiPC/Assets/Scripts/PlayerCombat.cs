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
    public GameObject Sword; // identifies the Sword object in game

    private int currentDamage = 10; // set the original damage dealt oto 10




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
            

            other.GetComponent<EnemyCombat>().TakeDamage(currentDamage); // updated so it takes the current damage, not just a set value
            
        }

        if (other.tag == "ArmourPickup")
        {
            ArmourPickup armourPickup = other.GetComponent<ArmourPickup>();
            if (armourPickup != null)
            {
                
                PickupArmour(armourPickup.armourPoints); // uses the pickuparmour function to increase the armour
                Destroy(other.gameObject); // destroys the armour object
            }
        }

        if (other.tag == "HealthPotionPickup")
        {
            HealthPotionPickup HealthPotionPickup = other.GetComponent<HealthPotionPickup>();
            if (HealthPotionPickup != null)
            {
                
                HealthPickupPotion(HealthPotionPickup.maxHealthIncrease, HealthPotionPickup.healthRestore); // pass the increase in health max and current as parameters
                Destroy(other.gameObject); // destroys the potion object
            }
        }

        if (other.tag =="DamagePotionPickup")
        {
            DamagePotionPickup damagePotionPickup = other.GetComponent<DamagePotionPickup>();
            if (damagePotionPickup != null)
            {
                
                DamagePotionPickup(damagePotionPickup.damageIncrease); // passes the damage increase as a parameter in the method to increase the damage
                Destroy(other.gameObject); // destroys the potion object
            }
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

        PlayerHealthSlider.value = currentHealth;
        PlayerArmourSlider.value = currentArmour;
    }


    public void PickupArmour(int armourPoints)
    {
        
        maxArmour += armourPoints; // increases max armour
        currentArmour += (armourPoints + 10); // increases current armour by a bit more than max is 

        
        currentArmour = Mathf.Clamp(currentArmour, 0, maxArmour); // stops current armour exceeding the max armour stat


        
        PlayerArmourSlider.maxValue = maxArmour; // sliders need updating again
        PlayerArmourSlider.value = currentArmour;
    }

    public void HealthPickupPotion(int maxHealthIncrease, int healthRestore)
    {
        
        maxHealth += maxHealthIncrease;
        currentHealth = Mathf.Min(currentHealth + healthRestore, maxHealth); // makes sure the health is between the min and max health

        
        PlayerHealthSlider.maxValue = maxHealth; // update sliders
        PlayerHealthSlider.value = currentHealth;
    }

    public void DamagePotionPickup(int damageIncrease)
    {
        
        currentDamage += damageIncrease;

        Debug.Log("Picked up damage potion. Current damage: " + currentDamage);
        StartCoroutine(ResetDamageAfterDelay(20f)); // trigger the start of the duration of the potion



    }

    private IEnumerator ResetDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // once the potion runs out the damage is set back to 10
        currentDamage = 10;
        Debug.Log("Current Damage: " + currentDamage);
    }
}
