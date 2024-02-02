using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // used to get the position of the player
    public float detectionRange = 10f; // the distance between the enemy and player in which the enemy will chase the player within
    public float moveSpeed = 3f; // move speed of the enemy
    public float pathUpdateCooldown = 1f; // used to adjust how often the path calculation is performed

    private Pathfinding pathfinding;
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private float lastPathUpdateTime;

    void Start()
    {
        // ensures the player reference is obtained correctly
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        

        pathfinding = new Pathfinding(227, 168); // initialise the grid 
        lastPathUpdateTime = Time.time;
        
    }

    void Update()
    {
        

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRange && Time.time - lastPathUpdateTime > pathUpdateCooldown)
        {
            // Update the path to the player
            List<Vector3> newPath = pathfinding.UpdatePathToPlayer(transform.position, player.position);

            // Move the enemy along the path
            if (newPath != null && newPath.Count > 0)
            {
                currentPath = newPath;
                currentPathIndex = 0;
                MoveEnemy();
                lastPathUpdateTime = Time.time; // Update the last update time
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
