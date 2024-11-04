using System.Collections;
using TMPro;
using UnityEngine;

public class WhackMoleAgent : GridAgentBase
{
    public GameObject molePrefab;
    public GameObject mole = null;
    public int Score;
    public TMP_Text scoreText;

    public override void RegisterEvents()
    {
        base.RegisterEvents();
        grid.OnGridChanged += CreateOrDestroyMole;
    }

    #region GamePlay


    void CreateOrDestroyMole(int x, int y, int value)
    {
        if (value == 1)
        {
            Vector3 worldPosition = XYToWorldPos(x, y);
            mole = Instantiate(molePrefab, worldPosition, Quaternion.identity);
        }
        else if (value == 0 && mole != null)
        {
            Destroy(mole);
            mole = null;
        }
        //Debug.Log("CreateMoles");
    }

    void MoleCameOutRandomly()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid.gridArray[i, j] == 1)
                {
                    grid.SetCell(i, j, 0);
                }
            }
        }
        int x = Random.Range(0, width);
        int y = Random.Range(0, height);
        grid.SetCell(x, y, 1);
    }

    IEnumerator MoleOutCoroutine()
    {
        while (true)
        {
            MoleCameOutRandomly();
            yield return new WaitForSeconds(1f);
        }
    }

    void HitTheMole()
    {
        Vector3 mousePos = GridWithWorldTool.GetMousePosFromWorld();
        WorldPosToXY(mousePos, out int x, out int y);
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Score--; // 超出边界，扣分
        }
        else if (grid.gridArray[x, y] == 1)
        {
            Score++; // 击中鼹鼠，得分
            grid.SetCell(x, y, 0); // 清除击中位置的鼹鼠
        }
        else
        {
            Score--; // 未击中，扣分
        }

        // 更新分数显示
        scoreText.text = "Score: " + Score;
    }
    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        StartCoroutine(MoleOutCoroutine());
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            HitTheMole();
        }
    }
    #endregion
}
