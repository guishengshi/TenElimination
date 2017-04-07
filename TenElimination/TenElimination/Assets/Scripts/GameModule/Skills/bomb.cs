using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 炸4x4方块
/// </summary>

public class Bomb : BaseSkill {
    private int mColumnNumber;
    private int mLineNumber;


    public Bomb(StoreItemData data, GameCenter gameCenter) : base(data, gameCenter) {
        mColumnNumber = int.Parse(data.para1);
        mLineNumber = int.Parse(data.para2);
        
    }

    protected override void InitEffect()
    {
        mEffect = Helper.GetEffect(mStoreItemData.effectName);
    }

    protected override void DoSkill()
    {
        DestroyBlock();
        mIsStart = true;
    }

    protected override void DoEffect()
    {
        mEffect.gameObject.SetActive(true);
    }

    private float mTimer = 0f;
    private bool mIsStart = false;

    protected override void OnUpdate(float timeDelta)
    {
        base.OnUpdate(timeDelta);
        if (mIsStart) {
            if (mTimer < 1f)
            {
                mTimer += timeDelta;
            }
            else
            {
                GameObject.Destroy(mEffect.gameObject);
                mIsStart = false;
            }
        }
        
    }

    private void DestroyBlock() {
        if (mColumnNumber == 2 && mLineNumber == 2) {
            List<BlockManager> list = null;
            Vector3 pos = mGameCenter.GetRandom2x2Block(out list);
            mEffect.transform.position = pos;
            for (int i = 0; i < list.Count; i++) {
                list[i].Dead();
            }
        }
        if (mColumnNumber == 4 && mLineNumber == 4) {
            List<BlockManager> list = null;
            Vector3 pos = mGameCenter.GetCenter4x4Blocks(out list);
            mEffect.transform.position = pos;
            for (int i = 0; i < list.Count; i++) {
                list[i].Dead();
            }
        }
        if (mColumnNumber == 6) {
            DestroyBlockFromLine();
        } else if (mLineNumber == 6) {
            DestroyBlockFromColumn();
        }
    }

    private void DestroyBlockFromLine() {
        
    }

    private void DestroyBlockFromColumn() {
        int column = Random.Range(0, 6);
        List<BlockManager> list = null;
        Vector3 pos = mGameCenter.GetBlocksFromColumn(column, out list);
        mEffect.transform.position = pos;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Dead();
        }
    }

}
