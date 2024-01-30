using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid {

    private int width; // width of grid 
    private int height; // height of grid
    private Vector3 originPosition; // the origin position of where the grid will start

    private float cellSize; // size of each cell
    private int[,] gridArray; // all of the cell coordinates in an array


    public Grid(int width, int height, float cellSize, Vector3 originPosition) { // assing all of the attributes
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width,height]; // create the 2D array of coordinates

       

        for (int x=0; x < gridArray.GetLength(0); x++) {  // this iterates through the 2D array, showing the position of the cells on screen 
            for (int y=0; y < gridArray.GetLength(1); y++) {
                
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1),Color.white,100f); // draws vertacle lines of the grid
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1,y),Color.white,100f);

            }
        }
        Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height),Color.white,100f); // draws horizontal lines of the grid
        Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height),Color.white,100f);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x,y) * cellSize + originPosition; // returns the world position of each cell based off its grid position
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y) { // out allows the return of multiple values from this single function
       x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
       y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize); // this converts a world position into a grid position

    }


    public void SetValue(int x, int y, int value) {
        if (x>= 0 && y >= 0 && x < width && y < height) { // validates the x and y values, so they can't be negative
            gridArray[x,y] = value;

        }
        
    }

    public void SetValue(Vector3 worldPosition, int value) { // sets the value of a grid position
        int x,y;
        GetXY(worldPosition, out x, out y);
        SetValue(x,y,value);

    }

    public int GetValue(int x, int y){ // get the grid position
        if (x>= 0 && y >= 0 && x < width && y < height) { // validates the x and y values, so they can't be negative
            return gridArray[x,y];

        } else {
            return 0;
        }

    }

    public int GetValue(Vector3 worldPosition) { // gets the value of a grid position in relation to the world position
        int x,y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x,y);

    }



    

    
}
