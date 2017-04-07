using UnityEngine;
using System.Collections;
using AVOSCloud;

public class InGameState : IState {
    private UIManager mUIManager;
    private uint mID;
    private StateManager mGameStateManager;

    public StateManager GameStateManager {
        get {
            return mGameStateManager;
        }
    }

    public InGameState(UIManager manager, UIState state) {
        mUIManager = manager;
        mID = (uint)state;
        InitStateManager();
    }


    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mUIManager.MInGameWindow.Show();
        mGameStateManager.SwitchState((uint)GameState.SelectMode, null, null);
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        mGameStateManager.StopCurrentState(null, null);
        User.SaveCurrentUserData(mUIManager.MCoreGame.MPlayerManager.MPlayerData);
        mUIManager.MCoreGame.MGameCenter.OnExit();
        InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
        window.MScoreShowController.UpdateScore(0);
        if (window.bgMusicSource.clip != null) {
            window.bgMusicSource.Stop();
        }
        window.scoreWindow.gameObject.SetActive(false);
        mUIManager.MInGameWindow.Hide();
        //mUIManager.MCoreGame.MGameCenter.OnExit();
    }

    public void OnUpdate(float timeDelta)
    {
        mUIManager.MCoreGame.MGameCenter.Update(timeDelta);
        mGameStateManager.OnUpdate(timeDelta);
    }

    public void OnFixUpdate(float timeDelta)
    {
        mUIManager.MCoreGame.MGameCenter.FixedUpdate(timeDelta);
        mGameStateManager.OnFixUpdate(timeDelta);
    }

    public void OnLateUpdate(float timeDelta)
    {
        mUIManager.MCoreGame.MGameCenter.FixedUpdate(timeDelta);
        mGameStateManager.OnLateUpdate(timeDelta);
    }

    private void InitStateManager() {
        mGameStateManager = new StateManager();
        mGameStateManager.RigisterState(new SelectModeState(mUIManager, GameState.SelectMode));
        mGameStateManager.RigisterState(new LoadingState(mUIManager, GameState.Loading));
        mGameStateManager.RigisterState(new ReadyState(mUIManager, GameState.Ready));
        mGameStateManager.RigisterState(new GamingState(mUIManager, GameState.Gaming));
        mGameStateManager.RigisterState(new PauseState (mUIManager, GameState.Pause));
        mGameStateManager.RigisterState(new OverState(mUIManager, GameState.Over));
        //mGameStateManager.RigisterState(new MatchState(mUIManager, GameState.Matching));
    }

}

public enum GameState { 
    SelectMode,
    Loading,
    Ready,
    Gaming,
    Pause,
    Over,
    Matching,
}

public class SelectModeState : IState 
{ 
     private UIManager mUIManager;
     private uint mID;


     public SelectModeState(UIManager manager, GameState state)
     {
         mUIManager = manager;
         mID = (uint)state;
     }

     public uint GetStateID()
     {
         return mID;
     }

     public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
     {
         mUIManager.MCoreGame.MGameCenter.OnSelectMode();
         InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
         window.mModeSelectWindow.gameObject.SetActive(true);
         window.maskImage.gameObject.SetActive(true);
     }

     public void OnExit(IState nextIState, object para1, object para2)
     {
         InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
         window.mModeSelectWindow.gameObject.SetActive(false);
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

public class LoadingState : IState
{

    private UIManager mUIManager;
    private uint mID;


    public LoadingState(UIManager manager, GameState state)
    {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
        window.maskImage.gameObject.SetActive(false);
        switch (mUIManager.MCoreGame.MGameCenter.GameMode) {
            case GameMode.CustomMode :
                mUIManager.MCoreGame.MGameCenter.CurrentBlockSpeed = CommonSetting.blockStartSpeedInCustomMode;
                window.bgMusicSource.clip = window.customModeMusic;
                window.bgMusicSource.loop = true;
                break;
            case GameMode.TransformMode :
                mUIManager.MCoreGame.MGameCenter.CurrentBlockSpeed = CommonSetting.blockStartSpeedInTransformMode;
                mUIManager.MCoreGame.MGameCenter.CurrentNumberChangeSpeed = CommonSetting.blockNumberChangeSpeed;
                window.bgMusicSource.clip = window.transformModeMusic;
                window.bgMusicSource.loop = true;
                break;
            case GameMode.TopSpeedMode :
                mUIManager.MCoreGame.MGameCenter.CurrentBlockSpeed = CommonSetting.blockStartSpeedInTopSpeedMode;
                mUIManager.MCoreGame.MGameCenter.CurrentNumberChangeSpeed = CommonSetting.blockNumberChangeSpeedInTopSpeedMode;
                window.bgMusicSource.clip = window.topSpeedModeMusic;
                window.bgMusicSource.loop = false;
                break;
        }
       
        mUIManager.MLoadingWindow.Show(2f, () => { manager.SwitchState((uint)GameState.Ready, null, null); });
        mUIManager.MCoreGame.MGameCenter.Loading();
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

public class ReadyState : IState
{

    private UIManager mUIManager;
    private uint mID;

    public ReadyState(UIManager manager, GameState state)
    {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mUIManager.MCoreGame.MGameCenter.Ready();
        InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
        window.mPointerImage.gameObject.SetActive(true);
        if (mUIManager.MCoreGame.MGameCenter.GameMode == GameMode.TopSpeedMode)
        {
            window.mTimeShow.text = SecondToTime((int)window.bgMusicSource.clip.length - 5);
        }
        window.MScoreShowController.UpdateScore(0);
        window.bgMusicSource.Play();
        Tween.PingPong(window, window.mPointerImage, 3f, 1f, false, () => {
            manager.SwitchState((uint)GameState.Gaming, null, null);
            window.mPointerImage.gameObject.SetActive(false);
        });
        
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

    private string SecondToTime(int second) {
        int minute = second / 60;
        int leftSecond = second % 60;
        string m = null;
        string s = null;
        if (minute >= 10)
        {
            m = minute.ToString();
        }
        else {
            m = "0" + minute.ToString();
        }
        if (leftSecond >= 10)
        {
            s = leftSecond.ToString();
        }
        else {
            s = "0" + leftSecond.ToString();
        }
        string result = m + ":" + s;
        return result;
    }
}

public class GamingState : IState {

    private UIManager mUIManager;
    private StateManager mStateManager;
    private uint mID;
    private float mTimer = 0f;
    private int mSecond = 0;
    private int mMinute = 0;
    private UnityEngine.UI.Text mTimeShow;

    public GamingState(UIManager manager, GameState state)
    {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID() {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mStateManager = manager;
        InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
        if (prevState.GetStateID() == (uint)GameState.Pause)
        {
            mUIManager.MCoreGame.MGameCenter.ContinueGame();
        }
        else
        {
            mUIManager.MCoreGame.MGameCenter.StartGame();
            mTimer = 0f;
            mTimeShow = window.mTimeShow;
            if (mUIManager.MCoreGame.MGameCenter.GameMode == GameMode.TopSpeedMode)
            {
                int sumSecond = (int)window.bgMusicSource.clip.length - 5;
                mMinute = sumSecond / 60;
                mSecond = sumSecond % 60;
            }
            else {
                mMinute = 0;
                mSecond = 0;
            }
            
        }
        if (!window.bgMusicSource.isPlaying) {
            window.bgMusicSource.Play();
        }
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        
    }

    public void OnUpdate(float timeDelta)
    {
        if (mTimer >= 1f)
        {
            if (mUIManager.MCoreGame.MGameCenter.GameMode == GameMode.TopSpeedMode)
            {
                DeTimer();
            }
            else {
                Timer();
            }
            mTimer = 0f;
        }
        else {
            mTimer += timeDelta;
        }
        
    }

    public void OnFixUpdate(float timeDelta)
    {
        
    }

    public void OnLateUpdate(float timeDelta)
    {
        
    }

    private void Timer() {
        mSecond++;
        if (mSecond >= 60) {
            mSecond = 0;
            mMinute++;
        }
        string minute;
        string second;
        if (mSecond >= 10)
        {
            second = mSecond.ToString();
        }
        else {
            second = "0" + mSecond.ToString();
        }
        if (mMinute >= 10)
        {
            minute = mMinute.ToString();
        }
        else {
            minute = "0" + mMinute.ToString();
        }
        mTimeShow.text = minute + ":" + second;
    }

    private void DeTimer() {
        mSecond--;
        if (mSecond < 0)
        {
            mSecond = 59;
            mMinute--;
        }
        string minute;
        string second;
        if (mSecond >= 10)
        {
            second = mSecond.ToString();
        }
        else
        {
            second = "0" + mSecond.ToString();
        }
        if (mMinute >= 10)
        {
            minute = mMinute.ToString();
        }
        else
        {
            minute = "0" + mMinute.ToString();
        }
        mTimeShow.text = minute + ":" + second;
        if (mMinute == 0 && mSecond == 0) {
            mUIManager.MCoreGame.MGameCenter.PauseGame();
            mStateManager.SwitchState((uint)GameState.Over, null, null);
        }
    }
}

public class PauseState : IState
{

    private UIManager mUIManager;
    private uint mID;


    public PauseState(UIManager manager, GameState state)
    {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mUIManager.MCoreGame.MGameCenter.PauseGame();
        InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
        if (window.bgMusicSource.isPlaying) {
            window.bgMusicSource.Pause();
        }
        window.mGrayImage.gameObject.SetActive(true);
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
        window.mGrayImage.gameObject.SetActive(false);
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

public class OverState : IState
{

    private UIManager mUIManager;
    private uint mID;

    public OverState(UIManager manager, GameState state)
    {
        mUIManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        InGameWindow window = mUIManager.MInGameWindow as InGameWindow;
        window.scoreWindow.gameObject.SetActive(true);
        switch (mUIManager.MCoreGame.MGameCenter.GameMode) {
            case GameMode.CustomMode :
                GetCustomScore(window);
                break;
            case GameMode.TransformMode :
                GetTransformScore(window);
                break;
            case GameMode.TopSpeedMode :
                GetTopSpeedScore(window);
                break;
        }
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

    private void GetCustomScore(InGameWindow window) {
        int currentScore = mUIManager.MCoreGame.MGameCenter.Score;
        int maxScore = mUIManager.MCoreGame.MPlayerManager.MPlayerData.customModeScore;
        window.currentScoreResultText.text = "分数：" + currentScore.ToString();
        if (currentScore > maxScore)
        {
            mUIManager.MCoreGame.MPlayerManager.MPlayerData.customModeScore = currentScore;
            window.maxScoreText.text = "最高分：" + currentScore.ToString();
        }
        else
        {
            window.maxScoreText.text = "最高分：" + maxScore.ToString();
        }
        int gold = currentScore / 10;
        window.goldText.text = gold.ToString();
        mUIManager.MCoreGame.MPlayerManager.MPlayerData.gold += gold;
        User.SaveCurrentUserData(mUIManager.MCoreGame.MPlayerManager.MPlayerData);
    }

    private void GetTransformScore(InGameWindow window)
    {
        int currentScore = mUIManager.MCoreGame.MGameCenter.Score;
        int maxScore = mUIManager.MCoreGame.MPlayerManager.MPlayerData.transformModeScore;
        window.currentScoreResultText.text = "分数：" + currentScore.ToString();
        if (currentScore > maxScore)
        {
            mUIManager.MCoreGame.MPlayerManager.MPlayerData.transformModeScore = currentScore;
            window.maxScoreText.text = "最高分：" + currentScore.ToString();
        }
        else
        {
            window.maxScoreText.text = "最高分：" + maxScore.ToString();
        }
        int gold = currentScore / 10;
        window.goldText.text = gold.ToString();
        mUIManager.MCoreGame.MPlayerManager.MPlayerData.gold += gold;
        User.SaveCurrentUserData(mUIManager.MCoreGame.MPlayerManager.MPlayerData);
    }

    private void GetTopSpeedScore(InGameWindow window)
    {
        int currentScore = mUIManager.MCoreGame.MGameCenter.Score;
        int maxScore = mUIManager.MCoreGame.MPlayerManager.MPlayerData.topSpeedModeScore;
        window.currentScoreResultText.text = "分数：" + currentScore.ToString();
        if (currentScore > maxScore)
        {
            mUIManager.MCoreGame.MPlayerManager.MPlayerData.topSpeedModeScore = currentScore;
            window.maxScoreText.text = "最高分：" + currentScore.ToString();
        }
        else
        {
            window.maxScoreText.text = "最高分：" + maxScore.ToString();
        }
        int gold = currentScore / 10 * 2;
        window.goldText.text = gold.ToString();
        mUIManager.MCoreGame.MPlayerManager.MPlayerData.gold += gold;
        User.SaveCurrentUserData(mUIManager.MCoreGame.MPlayerManager.MPlayerData);
    }
}

//public class MatchState : IState
//{
//    private StateManager mStateManager;
//    private UIManager mUIManager;
//    private uint mID;


//    public MatchState(UIManager manager, GameState state)
//    {
//        mUIManager = manager;
//        mID = (uint)state;
//    }

//    public uint GetStateID()
//    {
//        return mID;
//    }

//    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
//    {
//        mStateManager = manager;
//        mUIManager.MMessageBox.MIsInProgress = true;
//        mUIManager.MMessageBox.MMessage = "匹配中...";
//        mUIManager.MMessageBox.MIsShow = true;
//        if (!Helper.CheckNetworkConnecting())
//        {
//            mUIManager.UIStateManager.SwitchState((uint)UIState.MainMenu, null, null);
//            return;
//        }
//        Matching();
//    }

//    public void OnExit(IState nextIState, object para1, object para2)
//    {
//        mUIManager.MMessageBox.MIsInProgress = false;
//        mUIManager.MMessageBox.MIsShow = true;
//    }

//    public void OnUpdate(float timeDelta)
//    {
//        if (!Helper.CheckNetworkConnecting()) {
//            mUIManager.MMessageBox.MIsInProgress = false;
//            mUIManager.MMessageBox.MMessage = "网络断开";
//            mUIManager.MMessageBox.MIsShow = true;
//            mUIManager.UIStateManager.SwitchState((uint)UIState.MainMenu, null, null);
//        }
//    }

//    public void OnFixUpdate(float timeDelta)
//    {

//    }

//    public void OnLateUpdate(float timeDelta)
//    {

//    }

//    private void Matching() {
//        //确认匹配表中是否存在正在等待匹配的用户，不存在则进入表中等待，存在则与等待匹配的用户离开匹配列表
//        AVObject o = new AVObject("Matching");
//        AVQuery<AVObject> query = new AVQuery<AVObject>("Matching");
//        query.CountAsync().ContinueWith((t) => {
//            if (t.Result == 0)
//            {
//                o["userID"] = AVUser.CurrentUser.ObjectId;
//                o.SaveAsync();
//            }
//            else {
//                query.FirstAsync().ContinueWith((s) => {
//                    AVObject tempClass = new AVObject(s.Result.ObjectId);
//                    tempClass["userID"] = AVUser.CurrentUser.ObjectId;
//                    for (int i = 0; i < 5; i++) {
//                        for (int j = 0; j < 5; j++) {
//                            tempClass[i.ToString() + j.ToString()] = 0;
//                        }
//                    }
                    
//                });
//            }
//        });
//    }

//}
