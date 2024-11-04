using System;
using UnityEngine;

public class Grid
{
    private int width; // 网格的宽度
    private int height; // 网格的高度
    private int cellSize; // 单元格的大小
    private int halfCellSize; // 单元格大小的一半
    public int[,] gridArray; // 存储网格数据的二维数组

    public Action<int, int, int> OnGridChanged; // 网格变化时的事件

    // 构造函数，初始化网格的宽度、高度和单元格大小
    public Grid(int width, int height, int cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        halfCellSize = cellSize / 2;
        gridArray = new int[width, height];
    }

    // 设置指定单元格的值，并触发网格变化事件
    public void SetCell(int x = 0, int y = 0, int value = 0)
    {
        gridArray[x, y] = value; // 更新单元格的值
        if (OnGridChanged == null)
        {
            Debug.Log("OnGridChanged是空委托，请检查代码时序");
            return; // 空委托不执行
        }
        OnGridChanged.Invoke(x, y, value); // 调用网格变化事件
    }

    // 绘制网格到世界坐标中
    public void DrawGrid(GameObject parent, Vector3Int originWorldPos, int cellSize)
    {
        parent.transform.position = originWorldPos; // 设置父对象的世界位置
        GridWithWorldTool.DrawGridIntoWorld(gridArray, originWorldPos, cellSize); // 调用工具绘制网格
    }
}
