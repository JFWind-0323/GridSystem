using System;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int cellSize;
    private int halfCellSize;
    public int[,] gridArray;

    public Action<int, int, int> OnGridChanged;

    public Grid(int width, int height, int cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        halfCellSize = cellSize / 2;
        gridArray = new int[width, height];
    }

    public void SetCell(int x = 0, int y = 0, int value = 0)
    {
        gridArray[x, y] = value;
        if (OnGridChanged == null)
        {
            Debug.Log("OnGridChanged是空委托，请检查代码时序");
            return;
        }
        OnGridChanged.Invoke(x, y, value);
    }

    public void DrawGrid(GameObject parent, Vector3Int originWorldPos, int cellSize)
    {
        parent.transform.position = originWorldPos;
        GridWithWorldTool.DrawGridIntoWorld(gridArray, originWorldPos, cellSize);
    }
}
