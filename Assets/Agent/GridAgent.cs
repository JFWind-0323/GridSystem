using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
public class GridAgentBase : MonoBehaviour
{

    public Grid grid;
    public int width;
    public int height;
    public int cellSize;

    public Vector3Int originWorldPos;

    [ShowInInspector]
    public int[,] DebugGridArray;
    public virtual void Awake()
    {
        grid = new Grid(width, height, cellSize);
        transform.position = originWorldPos;
        GridWithWorldTool.DrawGridIntoWorld(grid.gridArray, transform.position, cellSize);
    }
    public virtual void Start()
    {
        RegisterEvents();
        grid.SetCell();
    }

    public virtual void Update()
    {
        // OnClickDebug();
    }
    private void UpdateDebugGridArray(int x, int y, int value)
    {
        DebugGridArray ??= new int[width, height];
        DebugGridArray[x, y] = value;
    }

    public virtual void RegisterEvents()
    {
        grid.OnGridChanged += UpdateDebugGridArray;
    }

    public virtual void OnClickDebug()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GridWithWorldTool.GetMousePosFromWorld();
            GridWithWorldTool.WorldPosToXY(mousePos, originWorldPos, cellSize, out int x, out int y);
            grid.SetCell(x, y, 1);
            Debug.Log(x + ", " + y);
        }
    }

    public Vector3 XYToWorldPos(int x, int y)
    {
        return GridWithWorldTool.XYToWorldPos(x, y, originWorldPos, cellSize);
    }

    public void WorldPosToXY(Vector3 worldPos, out int x, out int y)
    {
        GridWithWorldTool.WorldPosToXY(worldPos, originWorldPos, cellSize, out x, out y);
    }

    public virtual void SetCell(int x, int y, int value)
    {
        grid.gridArray[x, y] = value;
    }
}