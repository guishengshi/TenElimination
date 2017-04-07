using UnityEngine;
using System.Collections;
/// <summary>
/// 全局参数设置类
/// </summary>

public class CommonSetting {
    public const int scorePerBlock = 10;
    public const int maxBlockNumberPerColumn = 6;
    public const int maxBlockNumberPerLine = 6;
    public static Vector2 blockSize = new Vector2(1.81f, 1.78f);
    public static BlockPosition startBlockPosition = new BlockPosition(2, 6);
    public static Vector2 zeroBlockPosition = new Vector2(-4.525f, 0);
    public const float errorRange = 0.3f;
    public const int playerMaxStoreItems = 4;
    public const float blockStartSpeedInCustomMode = 1.5f;
    public const float blockStartSpeedInTransformMode = 1.5f;
    public const float blockStartSpeedInTopSpeedMode = 6f;
    public const float blockSpeedInsertAmount = 0.07f;
    public const float blockNumberChangeSpeed = 1.2f;           //变化一次所需的时间
    public const float blockNumberChangeSpeedInTopSpeedMode = 0.8f;
    public const int rankItemNumber = 10;
}
