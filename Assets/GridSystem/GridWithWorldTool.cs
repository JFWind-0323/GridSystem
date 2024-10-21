using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridWithWorldTool
{

    public static void DrawGridIntoWorld(int[,] gridArray, Vector3 origin, float cellSize)
    {
        int width = gridArray.GetLength(0);
        int height = gridArray.GetLength(1);

        for (int i = 0; i <= width; i++) // 注意这里的 <=
        {
            Vector3 startHorizontal = new Vector3(origin.x + i * cellSize, origin.y, 0);
            Vector3 endHorizontal = new Vector3(origin.x + i * cellSize, origin.y - height * cellSize, 0);
            Debug.DrawLine(startHorizontal, endHorizontal, Color.white, 2000f); // 绘制垂直线
        }

        for (int j = 0; j <= height; j++) // 注意这里的 <=
        {
            Vector3 startVertical = new Vector3(origin.x, origin.y - j * cellSize, 0);
            Vector3 endVertical = new Vector3(origin.x + width * cellSize, origin.y - j * cellSize, 0);
            Debug.DrawLine(startVertical, endVertical, Color.white, 2000f); // 绘制水平线
        }
    }

    public static Vector3 GetMousePosFromWorld()
    {

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return new Vector3(worldPos.x, worldPos.y, 0);

    }

    public static void WorldPosToXY(Vector3 worldPos, Vector3 originWorldPos, int cellSize, out int x, out int y)
    {
        int halfcellSize = cellSize / 2;
        x = (int)(worldPos.x - originWorldPos.x) / cellSize;
        y = -(int)(worldPos.y - originWorldPos.y) / cellSize;
    }

    public static Vector2 XYToWorldPos(int x, int y, Vector3 originWorldPos, int cellSize)
    {
        int halfcellSize = cellSize / 2;
        x = x * cellSize + halfcellSize;
        y = -(y * cellSize + halfcellSize);
        return new Vector3(x + originWorldPos.x, y + originWorldPos.y, 0);
    }
}