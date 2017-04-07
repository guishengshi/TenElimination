using UnityEngine;
using System.Collections;
/// <summary>
/// 封装数字随机算法
/// </summary>
public class NumberRandomArithmetic 
{
    private const int mMinNum = 1;
    private const int mMaxNum = 9;

    public static int RandomNumber() {
        int number = 0;
        number = Random.Range(mMinNum, mMaxNum);
        return number;
    }
}
