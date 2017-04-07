using UnityEngine;
using System.Collections;

public class MainMenuState : IState {
    private UIManager mUIManager;
    private uint mID;

    public MainMenuState(UIManager manager, UIState state) {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mUIManager.MMainWindow.Show();
        MainWindow window = mUIManager.MMainWindow as MainWindow;
        if (window.bgMusic.isPlaying) {
            return;
        }
        window.bgMusic.Play();
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        //if (nextIState.GetStateID() != (uint)UIState.Introduction)
            
        if (nextIState.GetStateID() == (uint)UIState.InGame) {
            mUIManager.MMainWindow.Hide();
            MainWindow window = mUIManager.MMainWindow as MainWindow;
            window.bgMusic.Stop();
        }
    }

    public void OnUpdate(float timeDelta)
    {
        mUIManager.MMainWindow.UpdateUI(timeDelta);
    }

    public void OnFixUpdate(float timeDelta)
    {
        
    }

    public void OnLateUpdate(float timeDelta)
    {
        
    }
}
