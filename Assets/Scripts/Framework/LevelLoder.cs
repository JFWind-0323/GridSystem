using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelLoder : MonoBehaviour
{
    public string levelName;
    public List<GameObject> GOList;
    public Dictionary<int, GameObject> GODictionary;
    public Transform gridParent;
    public Vector3Int originWorldPos;
    private int cellSize;
    public readonly string pathRoot = "Assets/Data/BoxLevel/";
    public int[,] levelGrid;

    void Awake()
    {
        InitList();
        InitDictFromList(GOList, out GODictionary);
    }

    void Start()
    {
        LevelData levelData = LoadLevel(levelName);
        AnalizeData(levelData, out cellSize);
        LoadGOfromData(levelGrid);
    }

    public LevelData LoadLevel(string levelName)
    {
        string path = Path.Combine(pathRoot, levelName);
        path += ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);
            Debug.Log("加载关卡 " + levelName + " 成功");
            return levelData;
        }
        Debug.LogError("没有找到该关卡文件 " + path);
        return null;
    }

    public void AnalizeData(LevelData levelData, out int cellSize)
    {
        int width;
        int height;
        int[] levelDataGrid;

        width = levelData.width;
        height = levelData.height;
        cellSize = levelData.cellSize;
        levelDataGrid = levelData.levelGrid;

        levelGrid = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                levelGrid[i, j] = levelDataGrid[i * width + j];
            }
        }
        Debug.Log("解析关卡数据成功");
    }

    void LoadGOfromData(int[,] levelGrid)
    {
        for (int x = 0; x < levelGrid.GetLength(0); x++)
        {
            for (int y = 0; y < levelGrid.GetLength(1); y++)
            {
                int id = levelGrid[x, y];
                CreatGameObject(x, y, id);
            }
        }
        Debug.Log("加载关卡物体成功");
    }

    void CreatGameObject(int x, int y, int id)
    {
        Vector3 tergetPos = GridWithWorldTool.XYToWorldPos(x, y, originWorldPos, cellSize);
        Instantiate(GODictionary[id], tergetPos, Quaternion.identity, gridParent);
    }

    void InitDictFromList<T>(List<T> list, out Dictionary<int, T> dict)
    {
        dict = new Dictionary<int, T>();
        for (int i = 0; i < list.Count; i++)
        {
            if (!dict.ContainsKey(i))
            {
                dict.Add(i, list[i]);
            }
        }
    }

    void PrintDict<Tkey, TValue>(Dictionary<Tkey, TValue> dict)
    {
        foreach (var item in dict)
        {
            Debug.Log(item.Key + " : " + item.Value);
        }
    }

    void InitList()
    {
        GOList = new List<GameObject>();
        GOList = Resources.LoadAll<GameObject>("Prefabs/BoxPfbs").ToList<GameObject>();
    }
}
