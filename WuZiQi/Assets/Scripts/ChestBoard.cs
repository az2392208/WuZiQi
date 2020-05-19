using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBoard : MonoBehaviour
{
    #region 单例
    private static ChestBoard _instance;
    public static ChestBoard Instacne
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Envirement").GetComponent<ChestBoard>();
            }
            return _instance;
        }
    }
    #endregion
    //谁的回合
    public ChestType turn = ChestType.black;
    //  棋盘
    public int[,] grid;
    //棋子
    public GameObject[] prefebs;
    //等待时间
    public float timer = 0;
    //是否能继续下棋
    public bool gamestart = true;

    private Stack<GameObject> blackStack = new Stack<GameObject>();
    private Stack<GameObject> whiteStack = new Stack<GameObject>();

    private void Start()
    {
        grid = new int[15, 15];
        if (GameObject.Find("Black") == null)
        {
            GameObject bb = new GameObject("Black");
        }
        if (GameObject.Find("White") == null)
        {
            GameObject ww = new GameObject("White");
            ww.transform.SetParent(null);
        }

        GameObject.Find("RetractChest").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(RetractChest);
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    /// <summary>
    /// 下棋的方法
    /// </summary>
    /// <param name="pos"> 棋子的位置</param>
    public void PlayChest(int[] pos)
    {
        if (!gamestart)
        {
            Debug.Log("游戏结束了");
            return;
        }
        if (pos[0] > 14 || pos[0] < 0)
            return;
        if (pos[1] > 14 || pos[1] < 0)
            return;
        if (grid[pos[0], pos[1]] != 0)
        {
            return;
        }

        //黑棋下棋
        switch (turn)
        {
            case ChestType.white:
                GameObject whiteChest = Instantiate(prefebs[1], new Vector3(pos[0] - 7, pos[1] - 7, 0), Quaternion.identity);

                whiteChest.transform.SetParent(GameObject.Find("White").transform);
                grid[pos[0], pos[1]] = 1;
                whiteStack.Push(whiteChest);
                break;
            case ChestType.black:
                GameObject blackChest = Instantiate(prefebs[0], new Vector3(pos[0] - 7, pos[1] - 7, 0), Quaternion.identity);

                blackChest.transform.SetParent(GameObject.Find("Black").transform);
                grid[pos[0], pos[1]] = 2;
                blackStack.Push(blackChest);
                break;
            default:
                break;
        }
        CheckOneLineWin(pos, 1, 0);
        CheckOneLineWin(pos, 0, 1);
        CheckOneLineWin(pos, 1, 1);
        CheckOneLineWin(pos, 1, -1);
        turn = (turn == ChestType.white) ? ChestType.black : ChestType.white;
    }
    #region  检测是否胜利了
    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <param name="pos">棋子的位置</param>
    /// <param name="i">x方向的偏移量</param>
    /// <param name="j">y方向的偏移</param>
    public void CheckOneLineWin(int[] pos, int i, int j)
    {
        int x = pos[0];
        int y = pos[1];
        int linkNum = 1;
        while (linkNum < 5)
        {
            if (!CanOneLine(x += i, y += j))
            {
                x = pos[0];
                y = pos[1];
                while (linkNum < 5)
                {
                    if (!CanOneLine(x -= i, y -= j))
                    {
                        break;
                    }
                    linkNum++;
                }
                break;
            }
            linkNum++;
        }
        if (linkNum >= 5)
        {
            gamestart = false;
        }
    }
    private bool CanOneLine(int x, int y)
    {
        if (x < 0 || x > 14)
        {
            return false;
        }
        if (y < 0 || y > 14)
        {
            return false;
        }
        if (grid[x, y] != (int)turn)
            return false;

        return true;
    }
    #endregion
    #region 悔棋方法
    public void RetractChest()
    {
        Debug.Log("可以了");
        if (blackStack.Count > 0 && whiteStack.Count > 0)
        {
            GameObject blackChest = blackStack.Pop();
            GameObject whiteChest = whiteStack.Pop();
            grid[(int)(blackChest.transform.position.x + 7.5f), (int)(blackChest.transform.position.y + 7.45f)] = 0;
            grid[(int)(whiteChest.transform.position.x + 7.5f), (int)(whiteChest.transform.position.y + 7.45f)] = 0;
            GameObject.Destroy(blackChest);
            GameObject.Destroy(whiteChest);
        }
    }
    #endregion
}
public enum ChestType
{
    watch,
    white,
    black
}
