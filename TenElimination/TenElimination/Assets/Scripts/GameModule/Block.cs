using UnityEngine;
using System.Collections;
/// <summary>
/// 脚本功能：方块模型类
/// </summary>
public class Block {
    public BlockType type;
    public int number;
    public int id;
    public static int defaultID = 0;
    public BlockPosition position;
    public Block(int id, int number, BlockType type) {
        this.id = id;
        this.number = number;
        this.type = type;
        this.position = new BlockPosition(CommonSetting.startBlockPosition.columnX, CommonSetting.startBlockPosition.lineY);
    }
}

public enum BlockType { 
    Normal,
    Change,
}

public enum BlockState { 
    Null = -1,
    Fall = 0,
    Stop = 1,
    Dead = 2,
    Pause = 3,
}
