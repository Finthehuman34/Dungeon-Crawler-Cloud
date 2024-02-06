

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;

    private bool showDebug;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        bool showDebug = true;
        if (showDebug)
        {
            debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Initialize debugTextArray
                    debugTextArray[x, y] = new TextMesh();

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                    // Ensure that debugTextArray[x, y] is not null
                    if (debugTextArray[x, y] != null)
                    {
                        debugTextArray[x, y].text = gridArray[x, y]?.ToString();
                    }
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                int x = eventArgs.x;
                int y = eventArgs.y;

                // Ensure that debugTextArray[eventArgs.x, eventArgs.y] is not null
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (debugTextArray[x, y] == null)
                    {
                        // Initialize debugTextArray
                        debugTextArray[x, y] = new TextMesh();
                    }

                    if (gridArray != null && x < gridArray.GetLength(0) && y < gridArray.GetLength(1))
                    {
                        debugTextArray[x, y].text = gridArray[x, y]?.ToString();
                    }
                }
            };
        }

        InitializeGrid(createGridObject);
    }

    private void HandleGridObjectChanged(object sender, OnGridObjectChangedEventArgs eventArgs)
    {
        if (showDebug && debugTextArray != null)
        {
            int x = eventArgs.x;
            int y = eventArgs.y;

            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                if (debugTextArray[x, y] == null)
                {
                    // Initialize debugTextArray
                    debugTextArray[x, y] = new TextMesh();
                }

                if (gridArray != null && x < gridArray.GetLength(0) && y < gridArray.GetLength(1))
                {
                    debugTextArray[x, y].text = gridArray[x, y]?.ToString();
                }
            }
        }
    }

    public void ClearGrid()
    {
    for (int x = 0; x < gridArray.GetLength(0); x++)
    {
        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
            SetGridObject(x, y, default(TGridObject)); // Set to default value instead of null
        }
    }

    // Ensure the event is not null before triggering it
    OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = -1, y = -1 });
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public float GetCellSize() {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x, y];
        } else {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

}
