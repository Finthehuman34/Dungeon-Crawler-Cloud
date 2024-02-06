using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float moveSpeed = 3f;

    public float pathUpdateCooldown = 5f;

    private Pathfinding pathfinding;
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private float lastPathUpdateTime;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        lastPathUpdateTime = Time.time;

        // Initial grid size and position
        int gridSizeX = 20;
        int gridSizeY = 20;
        Vector3 initialGridPosition = CalculateGridPosition();
        pathfinding = new Pathfinding(gridSizeX, gridSizeY, initialGridPosition);
    
    }

    void Update()
    {
        Debug.Log("Enemy Update");
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        
        Debug.Log("Before cooldown check");
        
        Debug.Log($"Last Path Update Time: {lastPathUpdateTime}");

        if (distanceToPlayer < detectionRange && Time.time - lastPathUpdateTime > pathUpdateCooldown)
        {
            Debug.Log("Updating path");
            UpdatePathfindingGrid();

            int enemyX, enemyY, playerX, playerY;
            pathfinding.GetGrid().GetXY(transform.position, out enemyX, out enemyY);
            pathfinding.GetGrid().GetXY(player.position, out playerX, out playerY);
            Debug.Log($"Enemy Grid Coordinates: ({enemyX}, {enemyY}), Player Grid Coordinates: ({playerX}, {playerY})");



            List<Vector3> newPath = pathfinding.FindPath(enemyX, enemyY, playerX, playerY);

            if (newPath != null && newPath.Count > 0)
            {
                currentPath = newPath;
                currentPathIndex = 0;
                MoveEnemy();
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
        // Calculate new grid size and position based on the enemy's position
        int gridSizeX = 20; // Adjust as needed
        int gridSizeY = 20; // Adjust as needed
        Vector3 gridPosition = CalculateGridPosition();

        // Re-initialize the grid with the new size and position
        pathfinding.InitializeGrid(gridSizeX, gridSizeY, gridPosition); // Modify this line
        Debug.Log("New grid made");
    }

    Vector3 CalculateGridPosition()
    {
        // Calculate the position for the new grid based on the enemy's position
        // Example: Offset the grid to be down and left of the enemy
        float offsetX = -1f; // Adjust as needed
        float offsetY = -1f; // Adjust as needed

        return transform.position + new Vector3(offsetX, offsetY, 0f);
    }

    void MoveEnemy()
    {
        if (currentPathIndex < currentPath.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPath[currentPathIndex], moveSpeed * Time.deltaTime);
            Debug.Log("Enemy moved");
            if (Vector3.Distance(transform.position, currentPath[currentPathIndex]) < 0.1f)
            {
                currentPathIndex++;
            }
        }
    }
}