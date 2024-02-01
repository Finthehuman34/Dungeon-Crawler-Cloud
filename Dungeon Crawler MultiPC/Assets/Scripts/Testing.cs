

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

public class Testing : MonoBehaviour {
    
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    
    private Pathfinding pathfinding;

    private void Start() {
        pathfinding = new Pathfinding(227, 162); // initialises the pathfinding algorithm, creating the grid of width 227 and height 162
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid()); // sets up the debugging of the steps of the grid 
        pathfindingVisual.SetGrid(pathfinding.GetGrid()); // sets up the debugging of the grid as a whole
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) { // when left click of the mouse
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition(); // gets the world position of where you click with your mouse
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y); // converts the world position of the mouse to the position on the grid
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y); // find a path between the start coordinates and where you clicked 
            if (path != null) { // if paths exist
                for (int i=0; i<path.Count - 1; i++) { // iterate through the paths
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 0.3f + Vector3.one * 0.15f, new Vector3(path[i+1].x, path[i+1].y) * 0.3f + Vector3.one * 0.15f, Color.green, 0.015f);
                    Debug.Log(path); // ^^^ draws a line from the start to the end of the path, following the nodes specified within the path
                }
            }
            
        }

        
    }

}
