

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14; // calculated using pythagorus

    

    private Grid<PathNode> grid;
    private List<PathNode> openList; // nodes queued for searching
    private List<PathNode> closedList; // nodes already searched

    public Pathfinding(int width, int height, Vector3 gridPosition)
    {
        
        InitializeGrid(width, height, gridPosition);
    }

    public void ClearGrid() {
        grid.ClearGrid();
    }

    public void InitializeGrid(int width, int height, Vector3 gridPosition)
    {
        if (grid != null)
        {
            grid.ClearGrid();
        }
        Debug.Log("New grid initialised in pathfinding script");
        grid = new Grid<PathNode>(width, height, 0.3f, gridPosition, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    

    public Grid<PathNode> GetGrid() { // get method for getting the grid
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        grid.GetXY(startWorldPosition, out int startX, out int startY); // get the xy coords of the start position in the path finding
        grid.GetXY(endWorldPosition, out int endX, out int endY); // get the xy coords of the end position in the path finding

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            return null; // if there's no path found then return null
        } else {
            List<Vector3> vectorPath = new List<Vector3>(); // else create a list of all the possible paths
            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f); // adds the paths to the list
            }
            return vectorPath;
        }
    }

    public List<Vector3> FindPath(int startX, int startY, int endX, int endY)
    {
        Debug.Log($"Finding path from ({startX}, {startY}) to ({endX}, {endY})");

        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        Debug.Log($"Start Node: {startNode}");
        Debug.Log($"End Node: {endNode}");

        Debug.Log($"Start Node Walkable: {startNode.isWalkable}, End Node Walkable: {endNode.isWalkable}");
        Debug.Log($"Start Node Position: {startNode.x}, {startNode.y}, End Node Position: {endNode.x}, {endNode.y}");

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            Debug.LogError("Invalid Start or End Node!");
            return null;
        }

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue; // Set to max value initially
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePathAsVector3(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode) || !neighbourNode.isWalkable)
                {
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<Vector3> CalculatePathAsVector3(PathNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(new Vector3(endNode.x, endNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(new Vector3(currentNode.cameFromNode.x, currentNode.cameFromNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public PathNode GetNode(int x, int y) { // Get node at specified coordinates
        return grid.GetGridObject(x, y);
    }

    private List<Vector3> CalculatePath(PathNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(endNode.GetWorldPosition()); // Assuming you have a method to get world position in PathNode
        PathNode currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode.GetWorldPosition());
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) { // Calculate the total cost from node 'a' to node 'b'
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) { // Find and return the node with the lowest fCost
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    public List<Vector3> UpdatePathToPlayer(Vector3 enemyPosition, Vector3 playerPosition)
    {
        grid.GetXY(enemyPosition, out int startX, out int startY);
        grid.GetXY(playerPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path != null && path.Count > 0)
        {
            // Process the path as needed (e.g., convert to world positions)
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
        else
        {
            return null;
        }
    }

}
