using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxLevel : MonoBehaviour
{
    public string jsonPath;
    private LevelData levelData;

    [FoldoutGroup("关卡数值")]
    public int width;

    [FoldoutGroup("关卡数值")]
    public int height;

    [FoldoutGroup("关卡数值")]
    public int cellSize;

    [FoldoutGroup("关卡数值")]
    public Vector3Int originWorldPos = new Vector3Int(-5, 5, 0);
    public Grid levelGrid;

    [FoldoutGroup("UI")]
    public TMP_Text curNumText;

    [FoldoutGroup("UI")]
    public TMP_Text curObjectName;

    public GameObject savePanel;
    private TMP_InputField inputField;

    private Button saveBtn;

    [Header("预制体")]
    public GameObject pfb;

    [Header("单元格的父")]
    public Transform gridParent;
    private Dictionary<int, GameObject> pfbDict;
    private Vector3 mousePos;

    [ShowInInspector, Header("当前数组")]
    public int[,] curGridArray;

    private GameObject[,] boxObjs;

    private int _curNum;
    public int curNum
    {
        get => _curNum;
        set
        {
            _curNum = value;
            ShowInput();
        }
    }

    #region Unity
    void Awake()
    {
        levelData = new LevelData();
        levelGrid = new(width, height, cellSize);
        curGridArray = new int[width, height];
        boxObjs = new GameObject[width, height];

        InitDict();
        InitData();
        curNum = 0;
    }

    void Start()
    {
        levelGrid.DrawGrid(gameObject, originWorldPos, cellSize);
        savePanel.SetActive(false);
        RegisterEvents();
        StartCoroutine(OnDrag());
    }

    private void Update()
    {
        GetInput();
        TrackMouse();
    }
    #endregion

    #region 初始化

    void InitData()
    {
        levelData.width = width;
        levelData.height = height;
        levelData.cellSize = cellSize;
        levelData.levelGrid = new int[width * height];
    }

    void InitSavePanel()
    {
        savePanel.SetActive(true);
        inputField = savePanel.transform.GetChild(0).GetComponent<TMP_InputField>();
        saveBtn = savePanel.transform.GetChild(1).GetComponent<Button>();
        saveBtn.onClick.AddListener(SaveLevel);
        Debug.Log(inputField);
    }

    void InitDict()
    {
        List<GameObject> goList = Resources
            .LoadAll<GameObject>("Prefabs/BoxPfbs")
            .ToList<GameObject>();
        pfbDict = new Dictionary<int, GameObject>();
        for (int i = 0; i < goList.Count; i++)
        {
            pfbDict.Add(i, goList[i]);
        }
    }

    void RegisterEvents()
    {
        levelGrid.OnGridChanged += UpdateDebugGridArray;
        levelGrid.OnGridChanged += DestroyObject;
        levelGrid.OnGridChanged += CreatObject;
    }

    #endregion

    #region 输入
    void TrackMouse()
    {
        pfb.transform.GetChild(0).position = mousePos;
    }

    void GetInput()
    {
        mousePos = GridWithWorldTool.GetMousePosFromWorld();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            curNum = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            curNum = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            curNum = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            curNum = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            curNum = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            InitSavePanel();
            StopAllCoroutines();
        }
        else
        {
            return;
        }

        if (Input.GetMouseButton(2))
        {
            Debug.Log(mousePos);
            Camera.main.transform.position = mousePos;
        }
    }

    void ShowInput()
    {
        curNumText.text = $"Current Number : {curNum.ToString()}";
        curObjectName.text = pfbDict[curNum].name;
        Instantiate(pfbDict[curNum], mousePos, Quaternion.identity, pfb.transform);
        Destroy(pfb.transform.GetChild(0).gameObject);
    }

    IEnumerator OnDrag()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                GridWithWorldTool.WorldPosToXY(
                    mousePos,
                    originWorldPos,
                    cellSize,
                    out int x,
                    out int y
                );
                if (!(x < 0 || x >= width || y < 0 || y >= height))
                {
                    SetCell(x, y, curNum);
                }
            }
            yield return new WaitForSeconds(0.04f);
        }
    }
    #endregion

    #region 数组操作
    private void UpdateDebugGridArray(int x, int y, int value)
    {
        curGridArray ??= new int[width, height];
        curGridArray[x, y] = value;
    }

    void SetCell(int x, int y, int value)
    {
        levelGrid.SetCell(x, y, value);
    }
    #endregion

    // #region 数组转换工具
    // T[] Array2DToArray<T>(T[,] array2D)
    // {
    //     int row = array2D.GetLength(0);
    //     int col = array2D.GetLength(1);
    //     int length = row * col;
    //     T[] array = new T[length];
    //     for (int i = 0; i < row; i++)
    //     {
    //         for (int j = 0; j < col; j++)
    //         {
    //             array[i * col + j] = array2D[i, j];
    //         }
    //     }
    //     return array;
    // }

    // T[,] ArrayToArray2D<T>(T[] array2D, int width, int height)
    // {
    //     T[,] array = new T[width, height];
    //     for (int i = 0; i < width; i++)
    //     {
    //         for (int j = 0; j < height; j++)
    //         {
    //             array[i, j] = array2D[i * height + j];
    //         }
    //     }
    //     return array;
    // }
    // #endregion

    #region 数据操作
    void SaveLevel()
    {
        levelData.levelGrid = curGridArray.ToArray();
        // levelData.levelGrid = Array2DToArray(curGridArray);
        string json = JsonUtility.ToJson(levelData, true);
        string path = jsonPath + inputField.text + ".json";
        File.WriteAllText(path, json);
        Debug.Log(path + " saved");
    }
    #endregion

    #region GameObject

    void CreatObject(int x, int y, int value)
    {
        Vector3 targetPos = GridWithWorldTool.XYToWorldPos(x, y, originWorldPos, cellSize);
        //Debug.Log(targetPos);
        boxObjs[x, y] = Instantiate(pfbDict[value], targetPos, Quaternion.identity, gridParent);
    }

    void DestroyObject(int x, int y, int value)
    {
        if (boxObjs[x, y] == null)
            return;
        Destroy(boxObjs[x, y]);
        boxObjs[x, y] = null;
    }
    #endregion
}
