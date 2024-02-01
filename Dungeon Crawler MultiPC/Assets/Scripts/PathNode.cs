

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PathNode {

    private Grid<PathNode> grid; // an object of the grid
    public int x; // x coordinate
    public int y; // y coordinate

    public int gCost; // gcost is the cost of the path from the start node to the current node
    public int hCost; // hcost is the heuristic estimate of the cost from the current node to the goal node
    public int fCost; // fcost = hcost + gcost

    public bool isWalkable; // checks if a node is walkable - this will be changed to function around walls
    public PathNode cameFromNode; // variable to check if you've come from a node, so you don't go back over that node

    public PathNode(Grid<PathNode> grid, int x, int y) { // initialising the nodes within the grid space
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost() { // calculates the fcost of a node
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable) { // sets a node to be walkable or not 
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
        return x + "," + y;
    }

}
