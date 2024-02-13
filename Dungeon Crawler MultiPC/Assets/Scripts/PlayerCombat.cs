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

    public float attackCooldown = 1f;
    private bool canAttack = true;

    public Collider2D SwordCollider;
    public GameObject SwordObject;
    private Transform SwordTransform;

    private void Start()
    {
        currentHealth = maxHealth;
        currentArmour = maxArmour;

        PlayerHealthSlider.maxValue = maxHealth;
        PlayerHealthSlider.value = currentHealth;

        PlayerArmourSlider.maxValue = maxArmour;
        PlayerArmourSlider.value = currentArmour;

        // Assign the SwordTransform from SwordObject
        SwordTransform = SwordObject.transform;

        if (SwordObject != null)
        {
            Transform childTransform = SwordObject.transform.Find("SwordColliderObject");
            if (childTransform != null)
            {
                SwordCollider = childTransform.GetComponent<Collider2D>();
            }
        }

        SwordObject.SetActive(false);
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            // Implement player defeat logic, like game over or respawn
            Debug.Log("Player defeated!");
        }

        // Update the sword position to follow the player with an offset
        Vector3 offset = new Vector3(0.07f, -0.06f, 0.0f); // Adjust the offset as needed
        SwordTransform.position = transform.position + offset;

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        SwordObject.SetActive(true);

        // Disable the sword collider temporarily to avoid multiple hits in a single swing
        SwordCollider.enabled = false;

        // Rotate the sword during the attack animation
        SwingSword();

        // Enable the sword collider after a short delay
        Invoke("EnableSwordCollider", 0.1f);

        // Detect enemies in the sword's collider
        Collider[] hitEnemies = Physics.OverlapBox(SwordCollider.bounds.center, SwordCollider.bounds.extents, SwordCollider.transform.rotation, LayerMask.GetMask("Enemy"));

        // Deal damage to each enemy in the sword's collider
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyCombat>().TakeDamage(10); // Adjust the damage value as needed
        }

        // Set the attack cooldown and update UI
        canAttack = false;
        Invoke("ResetAttackCooldown", attackCooldown);

        // Example: Play attack animation or sound
        Debug.Log("Player performed an attack!");

        if (SwordCollider != null)
        {
            SwordCollider.enabled = false;
        }
    }

    private void EnableSwordCollider()
    {
        SwordCollider.enabled = true;
    }

    private void ResetAttackCooldown()
    {
        canAttack = true;

        SwordObject.SetActive(false);
    }

    private void SwingSword()
    {
        // Get the player's rotation
        float playerRotation = transform.rotation.eulerAngles.z;

        // Calculate the target angle by adding 90 degrees to the player's rotation
        float targetAngle = transform.rotation.eulerAngles.z + 270;

        // Rotate the sword towards the target rotation using RotateTowards
        SwordTransform.rotation = Quaternion.Euler(0, 0, targetAngle);
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
