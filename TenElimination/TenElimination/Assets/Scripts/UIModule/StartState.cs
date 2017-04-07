using UnityEngine;
using System.Collections;

public class StartState : IState {
    private UIManager mUIManager;
    private uint mID;

    public StartState(UIManager manager, UIState state) {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mUIManager.MStartWindow.Show();
        StartWindow window = mUIManager.MStartWindow as StartWindow;
        window.bgMusic.Play();
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        mUIManager.MStartWindow.Hide();
        StartWindow window = mUIManager.MStartWindow as StartWindow;
        window.bgMusic.Stop();
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
