using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridAgentBase : MonoBehaviour
{
    public Grid grid;
    public int width;
    public int height;
    public int cellSize;

    public Vector3Int originWorldPos;

    [ShowInInspector]
    public int[,] DebugGridArray;

    //设置格子值
    public virtual void SetCell(int x, int y, int value)
    {
        grid.gridArray[x, y] = value;
    }

    // 事件注册
    public virtual void RegisterEvents()
    {
        grid.OnGridChanged += UpdateDebugGridArray;
    }

    #region 工具函数
    public Vector3 XYToWorldPos(int x, int y)
    {
        return GridWithWorldTool.XYToWorldPos(x, y, originWorldPos, cellSize);
    }

    public void WorldPosToXY(Vector3 worldPos, out int x, out int y)
    {
        GridWithWorldTool.WorldPosToXY(worldPos, originWorldPos, cellSize, out x, out y);
    }
    #endregion

    #region 调试函数


    // 点击调试
    public virtual void OnClickDebug()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GridWithWorldTool.GetMousePosFromWorld();
            GridWithWorldTool.WorldPosToXY(
                mousePos,
                originWorldPos,
                cellSize,
                out int x,
                out int y
            );
            grid.SetCell(x, y, 1);
            Debug.Log(x + ", " + y);
        }
    }

    // 更新调试数组
    private void UpdateDebugGridArray(int x, int y, int value)
    {
        DebugGridArray ??= new int[width, height];
        DebugGridArray[x, y] = value;
    }

    #endregion

    #region 基本生命周期
    public virtual void Awake()
    {
        grid = new Grid(width, height, cellSize);
        grid.DrawGrid(gameObject, originWorldPos, cellSize);
    }

    public virtual void Start()
    {
        RegisterEvents();
    }

    public virtual void Update()
    {
        // OnClickDebug();
    }
    #endregion
}
