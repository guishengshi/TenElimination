using UnityEngine;
using System.Collections;
/// <summary>
/// 方块消除算法
/// </summary>

public class BlockRemoveAlgorithm {
    public static bool IsRemove(int num1, int num2) {
        if (num1 + num2 == 10)
        {
            return true;
        }
        else {
            return false;
        }
    }
}
