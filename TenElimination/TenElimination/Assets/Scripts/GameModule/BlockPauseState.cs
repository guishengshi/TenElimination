using UnityEngine;
using System.Collections;

public class BlockPauseState : IState {
    //private BlockManager mBlockManager;
    private uint mID;

    public BlockPauseState(BlockManager manager, BlockState state) {
        //mBlockManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        
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
}
