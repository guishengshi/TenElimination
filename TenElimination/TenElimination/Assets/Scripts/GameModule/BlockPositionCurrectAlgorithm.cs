using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 方块位置矫正算法
/// </summary>

public class BlockPositionCurrectAlgorithm  {
    //private static float [] mCorrectNum = { -4.525f, -2.715f };-0.905f, 8.9f, 0
    //private static float mErrorRange = 0.1f;
    
    public static BlockPosition CurrentPos(Vector3 pos) {
        int column = Mathf.RoundToInt((pos.x - CommonSetting.zeroBlockPosition.x) / CommonSetting.blockSize.x);
        int line = Mathf.RoundToInt((pos.y - CommonSetting.zeroBlockPosition.y) / CommonSetting.blockSize.y);
        return new BlockPosition(column, line);
    }

    public static Vector3 CurrentPos(BlockPosition pos) {
        float x = pos.columnX * CommonSetting.blockSize.x + CommonSetting.zeroBlockPosition.x;
        float y = pos.lineY * CommonSetting.blockSize.y + CommonSetting.zeroBlockPosition.y;
        return new Vector3(x, y, 0);
    }
}
