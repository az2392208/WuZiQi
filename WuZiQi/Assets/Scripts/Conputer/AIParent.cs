using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIParent : Player
{
    /// <summary>
    /// AI的下棋方法
    /// </summary>
    public abstract override void PlayChest();


    //评分表
    public Dictionary<string, float> score = new Dictionary<string, float>();
}
