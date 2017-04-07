using UnityEngine;
using System.Collections;

public class IntroductionState : IState {
    private UIManager mUIManager;
    private uint mID;

    public IntroductionState(UIManager manager, UIState state) {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mUIManager.MIntroductionWindow.Show();
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        mUIManager.MIntroductionWindow.Hide();
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
