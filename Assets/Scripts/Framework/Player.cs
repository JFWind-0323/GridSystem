using System;
using UnityEngine;

public enum Diraction
{
    None = -1,
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
}

public class Player : MonoBehaviour, ICell
{
    public int curX;
    public int curY;

    public int targetX;
    public int targetY;
    public Diraction dir;

    void Start() { }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate() { }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            dir = Diraction.Up;
            OnInput();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            dir = Diraction.Down;
            OnInput();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            dir = Diraction.Left;
            OnInput();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dir = Diraction.Right;
            OnInput();
        }
        else
        {
            dir = Diraction.None;
            OnInput();
        }
    }

    void GetTargetStep(Diraction dir, out int targetX, out int targetY)
    {
        int xStep;
        int yStep;
        switch ((int)dir)
        {
            case 0:
                xStep = 0;
                yStep = 1;
                break;
            case 1:
                xStep = 0;
                yStep = -1;
                break;
            case 2:
                xStep = -1;
                yStep = 0;
                break;
            case 3:
                xStep = 1;
                yStep = 0;
                break;
            default:
                xStep = 0;
                yStep = 0;
                break;
        }
        targetX = curX + xStep;
        targetY = curY + yStep;
    }

    void OnInput()
    {
        int targetX;
        int targetY;
        GetTargetStep(dir, out targetX, out targetY);
    }

    public bool CheckTarget(int targetX, int targetY)
    {
        return false;
    }

    public void MoveToTarget(int targetX, int targetY) { }
}
