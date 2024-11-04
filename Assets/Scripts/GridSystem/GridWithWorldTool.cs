using UnityEngine;

public static class GridWithWorldTool
{
    // 将网格绘制到世界空间中
    public static void DrawGridIntoWorld(int[,] gridArray, Vector3 origin, float cellSize)
    {
        int width = gridArray.GetLength(0);
        int height = gridArray.GetLength(1);

        for (int i = 0; i <= width; i++) // 注意 <=
        {
            Vector3 startHorizontal = new Vector3(origin.x + i * cellSize, origin.y, 0);
            Vector3 endHorizontal = new Vector3(
                origin.x + i * cellSize,
                origin.y - height * cellSize,
                0
            );
            Debug.DrawLine(startHorizontal, endHorizontal, Color.white, 2000f); // 绘制垂直线
        }

        for (int j = 0; j <= height; j++) // 注意 <=
        {
            Vector3 startVertical = new Vector3(origin.x, origin.y - j * cellSize, 0);
            Vector3 endVertical = new Vector3(
                origin.x + width * cellSize,
                origin.y - j * cellSize,
                0
            );
            Debug.DrawLine(startVertical, endVertical, Color.white, 2000f); // 绘制水平线
        }
    }

    // 获取当前鼠标在世界空间中的位置
    public static Vector3 GetMousePosFromWorld()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return new Vector3(worldPos.x, worldPos.y, 0);
    }

    // 将世界坐标转换为网格的X和Y索引
    public static void WorldPosToXY(
        Vector3 worldPos,
        Vector3 originWorldPos,
        int cellSize,
        out int x,
        out int y
    )
    {
        int halfcellSize = cellSize / 2;
        x = (int)(worldPos.x - originWorldPos.x) / cellSize;
        y = -(int)(worldPos.y - originWorldPos.y) / cellSize;
    }

    // 将网格的X和Y索引转换为世界坐标
    public static Vector3 XYToWorldPos(int x, int y, Vector3 originWorldPos, int cellSize)
    {
        float halfcellSize = (float)cellSize / 2;
        float targetX = x * cellSize + halfcellSize + originWorldPos.x;
        float targetY = -(y * cellSize + halfcellSize) + originWorldPos.y;
        return new Vector3(targetX, targetY, 0);
    }
}
