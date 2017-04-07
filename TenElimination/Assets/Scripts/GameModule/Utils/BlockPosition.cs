using UnityEngine;
using System.Collections;
/// <summary>
/// 描述方块位置的工具类
/// </summary>
public class BlockPosition : Object {
    public int columnX;
    public int lineY;
    private static BlockPosition mZero = new BlockPosition(0, 0);
    private static int mMinColumnX = 0;
    private static int mMinLineY = 0;

    public static int MinColumnX {
        get {
            return mMinColumnX;
        }
    }

    public static int MaxColumnX {
        get {
            return mMinColumnX + CommonSetting.maxBlockNumberPerLine - 1;
        }
    }

    public static int MinLineY {
        get {
            return mMinLineY;
        }
    }

    public static int MaxLineY {
        get {
            return mMinLineY + CommonSetting.maxBlockNumberPerColumn - 1;
        }
    }

    public static BlockPosition Zero {
        get {
            return mZero;
        }
    }

    public BlockPosition() { 
    
    }

    public BlockPosition(int x, int y) {
        this.columnX = x;
        this.lineY = y;
    }

    public BlockPosition(Vector3 pos) {
        BlockPosition p = BlockPositionCurrectAlgorithm.CurrentPos(pos);
        this.columnX = p.columnX;
        this.lineY = p.lineY;
    }


    public static Vector3 BlockPositionToVector3(BlockPosition pos) {
        return BlockPositionCurrectAlgorithm.CurrentPos(pos);
    }


    public static BlockPosition Vector3ToBlockPosition(Vector3 pos) {
        return BlockPositionCurrectAlgorithm.CurrentPos(pos);
    }

    public Vector3 ToVector3() {
        return BlockPositionCurrectAlgorithm.CurrentPos(this);
    }

    public override bool Equals(object o)
    {
        BlockPosition pos = o as BlockPosition;
        if (pos.columnX == this.columnX && pos.lineY == this.lineY) {
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public bool IsNear(BlockPosition pos) {
        if (pos.columnX == this.columnX) {
            if ((pos.lineY == this.lineY + 1) || (pos.lineY == this.lineY - 1)) {
                return true;
            }
        }
        if (pos.lineY == this.lineY)
        {
            if ((pos.columnX == this.columnX + 1) || (pos.columnX == this.columnX - 1))
            {
                return true;
            }
        }
        return false;
    }

}
