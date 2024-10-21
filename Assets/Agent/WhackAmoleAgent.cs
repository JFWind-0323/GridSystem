using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;
public class WhackMoleAgent : GridAgentBase
{
    public GameObject molePrefab;
    public GameObject mole = null;
    public int Score;
    public TMP_Text scoreText;

    public override void Awake()
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

    public override void RegisterEvents()
    {
        base.RegisterEvents();
        grid.OnGridChanged += CreateOrDestroyMole;
    }

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
        if (grid.gridArray[x, y] == 1)
        {
            Score++;
            grid.SetCell(x, y, 0);
        }
        else
        {
            Score--;
        }
        scoreText.text = "Score: " + Score;
    }
}