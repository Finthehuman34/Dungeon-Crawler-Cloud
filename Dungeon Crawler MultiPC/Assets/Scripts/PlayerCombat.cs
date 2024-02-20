using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private int currentDamage = 10; // set the original damage dealt to 10

    public TextMeshProUGUI GoldCounterText; // links the text of the gold counter on screen to the variable
    private int GoldCounter = 0;

    public DeathScreenController deathScreenController; // reference to the death screen
    public WinScreenController WinScreenController; // reference to the win screen

    public int Kills = 0;






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

        Debug.Log("Kills:"+Kills);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            deathScreenController.ShowDeathScreen();
            Kills = 0;
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

    public void PickupGold(int amount)
    {
        GoldCounter += amount; // change the gold counter once gold is picked up
        UpdateGoldCounter(); // used to update the counter on screen

        
    }

    private void UpdateGoldCounter()
    {
        if (GoldCounterText != null)
        {
            GoldCounterText.text = "Gold: " + GoldCounter.ToString(); //  updates the counter on screen
        }
    }

    public void Kill() {
        Kills ++;
        Debug.Log("Kill trigger");

        if (Kills >= 21) {
            WinScreenController.ShowWinScreen();
            Debug.Log("WIN");
        }
    }
}
