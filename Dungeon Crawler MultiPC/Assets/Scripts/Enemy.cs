using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float moveSpeed = 3f; // Added moveSpeed variable

    private Pathfinding pathfinding;
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;

    void Start()
    {
        pathfinding = new Pathfinding(227, 168); // Initialize with the specified width and height
        // Other initialization...
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRange)
        {
            // Update the path to the player
            List<Vector3> newPath = pathfinding.UpdatePathToPlayer(transform.position, player.position);

            // Move the enemy along the path
            if (newPath != null && newPath.Count > 0)
            {
                currentPath = newPath;
                currentPathIndex = 0;
                MoveEnemy();
            }
        }
    }

    void MoveEnemy()
    {
        // Check if there are remaining nodes in the path
        if (currentPathIndex < currentPath.Count)
        {
            // Move towards the next node in the path
            transform.position = Vector3.MoveTowards(transform.position, currentPath[currentPathIndex], moveSpeed * Time.deltaTime);

            // Check if the enemy has reached the current node
            if (Vector3.Distance(transform.position, currentPath[currentPathIndex]) < 0.1f)
            {
                currentPathIndex++;
            }
        }
    }
}
