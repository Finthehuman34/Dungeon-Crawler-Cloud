using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f; // detection range (in relation to distance from player) in which it will trigger the path finding
    public float moveSpeed = 3f; // movement speed of the enemy

    public float pathUpdateCooldown = 5f; // adds a cooldown to how often the grid and path are updated

    private Pathfinding pathfinding;
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private float lastPathUpdateTime;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player"); // attatches the player in game to the player game object
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        lastPathUpdateTime = Time.time; 

        // this initialises the grid, with its width, height, and position from which it is created
        int gridSizeX = 20;
        int gridSizeY = 20;
        Vector3 initialGridPosition = CalculateGridPosition();
        pathfinding = new Pathfinding(gridSizeX, gridSizeY, initialGridPosition);
    
    }

    void Update()
    {
        Debug.Log("Enemy Update");
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // distance between enemy and player
        
        
        Debug.Log("Before cooldown check");
        
        Debug.Log($"Last Path Update Time: {lastPathUpdateTime}");

        // checks if the player is within range, and if the path hasn't been updated in the cooldown time
        if (distanceToPlayer < detectionRange && Time.time - lastPathUpdateTime > pathUpdateCooldown)
        {
            Debug.Log("Updating path");

            // gets grid coordinates for the enemy and player
            int enemyX, enemyY, playerX, playerY;
            pathfinding.GetGrid().GetXY(transform.position, out enemyX, out enemyY);
            pathfinding.GetGrid().GetXY(player.position, out playerX, out playerY);
            Debug.Log($"Enemy Grid Coordinates: ({enemyX}, {enemyY}), Player Grid Coordinates: ({playerX}, {playerY})");

            // finds a path using grid coordinates
            List<Vector3> newPath = pathfinding.FindPath(enemyX, enemyY, playerX, playerY);
            
            if (newPath != null && newPath.Count > 0)
            {
                currentPath = newPath;
                currentPathIndex = 0;
                MoveEnemy(); // if the path is found it moves the enemy
                lastPathUpdateTime = Time.time;
                Debug.Log("Path found!");
            }
            else
            {
                Debug.Log("No path found!");
            }
        }
        Debug.Log("After cooldown check");
    }

    private void UpdatePathfindingGrid()
    {
        Debug.Log("Updating pathfinding grid");
        // creates a new grid 
        int gridSizeX = 20; 
        int gridSizeY = 20; 
        Vector3 gridPosition = CalculateGridPosition();

        // reinitializes the grid with the new size and position
        pathfinding.InitializeGrid(gridSizeX, gridSizeY, gridPosition); 
        Debug.Log("New grid made");
    }

    Vector3 CalculateGridPosition()
    {
        // this is where the grid is offsetted down and to the left of the enemy, so the enemy is properly in the grid
        float offsetX = -1f; 
        float offsetY = -1f; 

        return transform.position + new Vector3(offsetX, offsetY, 0f);
    }

    void MoveEnemy()
    {
        if (currentPathIndex < currentPath.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPath[currentPathIndex], moveSpeed * Time.deltaTime); // actually moves the enemy on the path
            Debug.Log("Enemy moved");
            if (Vector3.Distance(transform.position, currentPath[currentPathIndex]) < 0.1f)
            {
                currentPathIndex++;
            }
        }
    }
}