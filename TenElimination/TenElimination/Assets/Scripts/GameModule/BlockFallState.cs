using UnityEngine;
using System.Collections;

public class BlockFallState : IState {
    private BlockManager mBlockManager;
    private uint mID;
    private float mCounter = 0;

    public BlockFallState(BlockManager manager, BlockState state) {
        mBlockManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mTimer = 0f;
        mPreTime = 0f;
        mIsEnd = false;
        //Vector3 start = mBlockManager.MTransform.position;
        //Tween.StraightTransform(start, Vector3.down, EndCall, mBlockManager.blockSpeed, mBlockManager);
        //Tween.TransformOnlyControlOneDirection(Vector3.down, EndCall, mBlockManager.blockSpeed, mBlockManager);
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        mCounter = 0;
        mIsEnd = true;
    }

    public void OnUpdate(float timeDelta)
    {
        if (Mathf.Abs (mBlockManager.MTransform.position.y - CommonSetting.zeroBlockPosition.y) < 0.1f) {
            mBlockManager.MBlockData.position.lineY = 0;
            mBlockManager.MStateManager.SwitchState((uint)BlockState.Stop, mBlockManager.MBlockData.position, mBlockManager.BlockColliders);
        }
        if (mBlockManager.MGameCenter.GameMode == GameMode.TransformMode || mBlockManager.MGameCenter.GameMode == GameMode.TopSpeedMode)
        {
            if (mCounter <= mBlockManager.MGameCenter.CurrentNumberChangeSpeed)
            {
                mCounter += timeDelta;
            }
            else
            {
                mCounter = 0f;
                mBlockManager.ChangeNumber();
            }
        }
        if (mTimer >= 1f)
        {
            mTimer = 0f;
            mPreTime = 0f;
        }
        else {
            mTimer += Time.deltaTime;
            float deltaY = Mathf.Lerp(0, mBlockManager.MGameCenter.CurrentBlockSpeed, mTimer / 1f) - Mathf.Lerp(0, mBlockManager.MGameCenter.CurrentBlockSpeed, mPreTime / 1f);
            mBlockManager.MTransform.position += Vector3.down * deltaY;
            mPreTime = mTimer;
        }
    }

    private float mTimer = 0f;
    private float mPreTime = 0f;

    public void OnFixUpdate(float timeDelta)
    {
        
    }

    public void OnLateUpdate(float timeDelta)
    {
        
    }

    private bool mIsEnd = false;

    private bool EndCall() { 
        return mIsEnd;
    }
}
