using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 5f;
    public float roamRange = 3f;
    public float moveSpeed = 3f;

    private bool isChasing = false;
    private Vector2 roamPoint;

    private void Start()
    {
        // Set an initial random roam point
        SetRandomRoamPoint();
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange)
        {
            // Player is within the chase range, start chasing
            isChasing = true;
        }
        else if (isChasing)
        {
            // Player is outside the chase range, but the enemy was chasing
            isChasing = false;
            SetRandomRoamPoint();
        }

        if (isChasing)
        {
            // Enemy is chasing the player
            ChasePlayer();
        }
        else
        {
            // Enemy is not chasing, roam around
            RoamAround();
        }
    }

    private void ChasePlayer()
    {
        // Calculate the direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the enemy towards the player
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void RoamAround()
    {
        // Calculate the direction towards the roam point
        Vector2 direction = (roamPoint - (Vector2)transform.position).normalized;

        // Move the enemy towards the roam point
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the roam point
        if (Vector2.Distance(transform.position, roamPoint) < 0.1f)
        {
            // Set a new random roam point
            SetRandomRoamPoint();
        }
    }

    private void SetRandomRoamPoint()
    {
        // Set a new random roam point within the roam range
        roamPoint = (Vector2)transform.position + Random.insideUnitCircle * roamRange;
    }
}