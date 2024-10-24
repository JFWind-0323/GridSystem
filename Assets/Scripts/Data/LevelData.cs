using System;
using Sirenix.Reflection.Editor;

[Serializable]
public class LevelData
{
    public int width;
    public int height;
    public int cellSize;
    public int[] levelGrid;
}
