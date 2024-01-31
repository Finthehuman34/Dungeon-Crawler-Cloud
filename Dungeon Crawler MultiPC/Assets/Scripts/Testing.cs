

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class Testing : MonoBehaviour {
    
    


    private Pathfinding pathfinding;

    private void Start() {
        pathfinding = new Pathfinding(227, 162);
        
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(3, -2, x, y);
            Debug.Log(mouseWorldPosition);
            Debug.Log(path);
            if (path != null) {
                for (int i=0; i<path.Count - 1; i++) {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 5f + Vector3.one * 2.5f, new Vector3(path[i+1].x, path[i+1].y) * 5f + Vector3.one * 2.5f, Color.green, 2.5f);
                    Debug.Log("Path");
                }
            }
            
        }

        
    }

}
