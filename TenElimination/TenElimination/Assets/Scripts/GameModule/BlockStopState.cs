using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockStopState : IState {
    private BlockManager mBlockManager;
    private uint mID;

    public BlockStopState(BlockManager manager, BlockState state) {
        mBlockManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        BlockPosition pos = (BlockPosition)para1;
        mBlockManager.MTransform.position = pos.ToVector3();
        mBlockManager.MBlockData.position.columnX = pos.columnX;
        mBlockManager.MBlockData.position.lineY = pos.lineY;
        if (pos.lineY > BlockPosition.MaxLineY) {
            InGameState state = mBlockManager.MGameCenter.MCoreGame.MUIManager.UIStateManager.GetState((uint)UIState.InGame) as InGameState;
            state.GameStateManager.SwitchState((uint)GameState.Over, mBlockManager.MGameCenter.Score, null);
            return;
        }
        mBlockManager.OnEnterStopState();
        List<BlockManager> removeBlocks = null;
        if (para2 != null)
        {
            List<BlockManager> blockColloders = para2 as List<BlockManager>;
            
            if (IsRemove(blockColloders, out removeBlocks))
            {
                for (int i = 0; i < removeBlocks.Count; i++)
                {
                    removeBlocks[i].Dead();
                }
            }
            else
            {
                if (mBlockManager.MGameCenter.GameMode == GameMode.TopSpeedMode && mBlockManager.MBlockData.position.lineY >= 3)
                {
                    mBlockManager.MGameCenter.ThreeLines();
                    mBlockManager.MBlockData.position.lineY -= 1;
                    mBlockManager.ResetPos();
                }
            }
            
        }
        else {
            if (IsRemove(out removeBlocks))
            {
                for (int i = 0; i < removeBlocks.Count; i++)
                {
                    removeBlocks[i].Dead();
                }
            }
            else {
                if (mBlockManager.MGameCenter.GameMode == GameMode.TopSpeedMode && mBlockManager.MBlockData.position.lineY >=3)
                {
                    mBlockManager.MGameCenter.ThreeLines();
                    mBlockManager.MBlockData.position.lineY -= 1;
                    mBlockManager.ResetPos();
                }
            }
        }
        mBlockManager.BlockColliders.Clear();
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        
    }

    public void OnUpdate(float timeDelta)
    {
        
    }

    public void OnFixUpdate(float timeDelta)
    {
        
    }

    public void OnLateUpdate(float timeDelta)
    {
        
    }


    private bool IsRemove(List <BlockManager> blockColliders, out List <BlockManager> removeBlocks) {
        bool isRemove = false;
        int num = mBlockManager.MBlockData.number;
        removeBlocks = new List<BlockManager>();
        BlockPosition thisPos = mBlockManager.MBlockData.position;
        for (int i = 0; i < blockColliders.Count; i++)
        {
            BlockPosition colliderPos = blockColliders[i].MBlockData.position;
            
            if (thisPos.IsNear(colliderPos) && BlockRemoveAlgorithm.IsRemove(num, blockColliders[i].MBlockData.number)) {
                removeBlocks.Add(blockColliders[i]);
                isRemove = true;
            }
        }
        if (isRemove) {
            removeBlocks.Add(mBlockManager);
        }
        return isRemove;
    }

    private bool IsRemove(out List <BlockManager> removeBlocks) {
        bool isRemove = false;
        int num = mBlockManager.MBlockData.number;
        BlockPosition thisPos = mBlockManager.MBlockData.position;
        removeBlocks = new List<BlockManager>();
        if (thisPos.columnX != BlockPosition.MinColumnX)
        {
            BlockManager leftBlock = mBlockManager.MGameCenter.GetBlock(new BlockPosition(thisPos.columnX - 1, thisPos.lineY));
            if (leftBlock != null && BlockRemoveAlgorithm.IsRemove(num, leftBlock.MBlockData.number))
            {
                isRemove = true;
                removeBlocks.Add(leftBlock);
            }
        }
        if (thisPos.columnX != BlockPosition.MaxColumnX)
        {
            BlockManager rightBlock = mBlockManager.MGameCenter.GetBlock(new BlockPosition(thisPos.columnX + 1, thisPos.lineY));
            if (rightBlock != null && BlockRemoveAlgorithm.IsRemove(num, rightBlock.MBlockData.number))
            {
                isRemove = true;
                removeBlocks.Add(rightBlock);
            }
        }
        if (thisPos.lineY != BlockPosition.MinLineY)
        {
            BlockManager downBlock = mBlockManager.MGameCenter.GetBlock(new BlockPosition(thisPos.columnX, thisPos.lineY - 1));
            if (downBlock != null && BlockRemoveAlgorithm.IsRemove(num, downBlock.MBlockData.number))
            {
                isRemove = true;
                removeBlocks.Add(downBlock);
            }
        }
        if (thisPos.lineY != BlockPosition.MaxLineY)
        {
            BlockManager upBlock = mBlockManager.MGameCenter.GetBlock(new BlockPosition(thisPos.columnX, thisPos.lineY + 1));
            if (upBlock != null && BlockRemoveAlgorithm.IsRemove(num, upBlock.MBlockData.number))
            {
                isRemove = true;
                removeBlocks.Add(upBlock);
            }
        }
        if (isRemove) {
            removeBlocks.Add(mBlockManager);
        }
        return isRemove;
    }

    //private bool IsRemove(out List <BlockManager> removeBlocks) {
    //    bool isRemove = false;
    //    int num = mBlockManager.MBlockData.number;
    //    removeBlocks = new List<BlockManager>();
    //    Vector3 leftBlockPos = mBlockManager.MTransform.position - new Vector3 (mBlockManager.blockSize.x, 0, 0);
    //    Vector3 rightBlockPos = mBlockManager.MTransform.position + new Vector3 (mBlockManager.blockSize.x, 0, 0);
    //    BlockManager leftBlockManager = null;
    //    BlockManager rightBlockManager = null;
    //    leftBlockManager = mBlockManager.MGameCenter.GetBlock(leftBlockPos);
    //    if (leftBlockManager != null && BlockRemoveAlgorithm.IsRemove(num, leftBlockManager.MBlockData.number)) {
    //        isRemove = true;
    //        removeBlocks.Add(leftBlockManager);
    //    }
    //    rightBlockManager = mBlockManager.MGameCenter.GetBlock(rightBlockPos);
    //    if (rightBlockManager != null && BlockRemoveAlgorithm.IsRemove(num, rightBlockManager.MBlockData.number))
    //    {
    //        isRemove = true;
    //        removeBlocks.Add(rightBlockManager);
    //    }
    //    if (mBlockManager.MTransform.position.y > mBlockManager.endPostionY) {
    //        Vector3 downBLockPos = mBlockManager.MTransform.position - new Vector3(0, mBlockManager.blockSize.x, 0);
    //        BlockManager downBlockManager = null;
    //        downBlockManager = mBlockManager.MGameCenter.GetBlock(downBLockPos);
    //        if (downBlockManager != null && BlockRemoveAlgorithm.IsRemove(num, downBlockManager.MBlockData.number))
    //        {
    //            isRemove = true;
    //            removeBlocks.Add(downBlockManager);
    //        }
    //    }
    //    if (isRemove) {
    //        removeBlocks.Add(mBlockManager);
    //    }
    //    return isRemove;
    //}
}
