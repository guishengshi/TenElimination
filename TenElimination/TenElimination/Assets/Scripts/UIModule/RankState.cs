﻿using UnityEngine;
using System.Collections;

public class RankState : IState {
    private UIManager mUIManager;
    private uint mID;

    public RankState(UIManager manager, UIState state) {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mUIManager.MRankWindow.Show();
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        mUIManager.MRankWindow.Hide();
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
