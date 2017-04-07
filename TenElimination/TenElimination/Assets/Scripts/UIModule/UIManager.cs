using UnityEngine;
using System.Collections;
using AVOSCloud;

public class UIManager : ICommonState {
    private CoreGame mCoreGame;
    //UI状态机 
    private StateManager mUIStateManager;
    private BaseWindow mMainWindow;
    private BaseWindow mInGameWindow;
    private BaseWindow mStartWindow;
    private BaseWindow mStoreWindow;
    private BaseWindow mRankWindow;
    private BaseWindow mIntroductionWindow;
    private NotificationCenter mNotificationCenter;
    private MessageBox mMessageBox;
    private LoadingWindow mLoadingWindow;

    public LoadingWindow MLoadingWindow {
        get {
            return mLoadingWindow;
        }
    }

    public MessageBox MMessageBox {
        get {
            return mMessageBox;
        }
    }

    public CoreGame MCoreGame {
        get {
            return mCoreGame;
        }
    }

    public BaseWindow MMainWindow
    {
        get { return mMainWindow; }
    }

    public BaseWindow MInGameWindow {
        get {
            return mInGameWindow;
        }
    }

    public BaseWindow MStartWindow {
        get {
            return mStartWindow;
        }
    }

    public BaseWindow MStoreWindow {
        get {
            return mStoreWindow;
        }
    }

    public BaseWindow MRankWindow {
        get {
            return mRankWindow;
        }
    }

    public BaseWindow MIntroductionWindow {
        get {
            return mIntroductionWindow;
        }
    }

    public StateManager UIStateManager {
        get {
            return mUIStateManager;
        }
    }


    public UIManager (CoreGame game) {
        mCoreGame = game;
    }

    public void Awake()
    {
        InitUIComponent();
        InitStateManager();
        InitNotificationCenter();
    }

    public void Start()
    {
        mUIStateManager.SwitchState((uint)UIState.Start, null, null);
    }

    public void Update(float timeDelta)
    {
        mMainWindow.UpdateUI(timeDelta);
        mInGameWindow.UpdateUI(timeDelta);
        mStartWindow.UpdateUI(timeDelta);
        mStoreWindow.UpdateUI(timeDelta);
        mRankWindow.UpdateUI(timeDelta);
        mIntroductionWindow.UpdateUI(timeDelta);
        mMessageBox.UpdateMessageBox(timeDelta);
        mLoadingWindow.UpdateLoadingWindow(timeDelta);
        mUIStateManager.OnUpdate(timeDelta);
        if (mIsEnterMainMenu) {
            mUIStateManager.SwitchState((uint)UIState.MainMenu, null, null);
            mIsEnterMainMenu = false;
        }
    }

    public void LateUpdate(float timeDelta)
    {
        mUIStateManager.OnLateUpdate(timeDelta);
    }

    public void FixedUpdate(float timeDelta)
    {
        mUIStateManager.OnFixUpdate(timeDelta);
    }

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        
    }

    private void InitUIComponent() {
        GameObject g = Helper.GetPrefabTypeOfGameObject("Canvas") as GameObject;
        GameObject startWindow = Helper.GetPrefabTypeOfGameObject("StartWindow");
        Helper.SetParent(startWindow.transform, g.transform);
        startWindow.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        mStartWindow = startWindow.GetComponent<StartWindow>();
        mStartWindow.Init();
        StartWindow startW = mStartWindow as StartWindow;
        startW.MRegisterWindow.OnRegisterInProgressEvent += () =>
        {
            mMessageBox.MIsInProgress = true;
            mMessageBox.MMessage = RegisterWindow.mRegisterInProgressStr;
            mMessageBox.MIsShow = true;
        };
        startW.MRegisterWindow.OnRegisterSuccessEvent += () =>
        {
            mMessageBox.MIsInProgress = false;
            mMessageBox.MMessage = RegisterWindow.mRegisterSuccessStr;
            mMessageBox.MIsShow = true;
        };
        startW.MRegisterWindow.OnRegisterFailedEvent += () =>
        {
            mMessageBox.MIsInProgress = false;
            mMessageBox.MMessage = RegisterWindow.mRegisterFailedStr;
            mMessageBox.MIsShow = true;
        };
        startW.MLoginWindow.OnLoginInProgressEvent += () =>
        {
            mMessageBox.MIsInProgress = true;
            mMessageBox.MMessage = LoginWindow.OnLoginInProgressStr;
            mMessageBox.MIsShow = true;
        };
        startW.MLoginWindow.OnLoginFailedEvent += () =>
        {
            mMessageBox.MIsInProgress = false;
            mMessageBox.MMessage = LoginWindow.OnLoginFailedStr;
            mMessageBox.MIsShow = true;
        };
        startW.MLoginWindow.OnLoginSuccessEvent += () =>
        {
            mMessageBox.MIsShow = false;
        };

        GameObject mainWindow = Helper.GetPrefabTypeOfGameObject("MainWindow");
        Helper.SetParent(mainWindow.transform, g.transform);
        mainWindow.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        mMainWindow = mainWindow.GetComponent<MainWindow>();
        MainWindow window = mMainWindow as MainWindow;
        mMainWindow.Init();
        window.StartGameCall = () => { mUIStateManager.SwitchState((uint)UIState.InGame, null, null); };
        window.RankCall = () => { mUIStateManager.SwitchState((uint)UIState.Rank, null, null); };
        window.GameIntroductionCall = () => { mUIStateManager.SwitchState((uint)UIState.Introduction, null, null); };
        window.StoreCall = () => { mUIStateManager.SwitchState((uint)UIState.Store, null, null); };
        window.GameOverCall = () => { Application.Quit(); };

        GameObject inGameWindow = Helper.GetPrefabTypeOfGameObject("InGameWindow");
        Helper.SetParent(inGameWindow.transform, g.transform);
        inGameWindow.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        mInGameWindow = inGameWindow.GetComponent<InGameWindow>();
        mInGameWindow.Init();
        InGameWindow inGameWin = mInGameWindow as InGameWindow;
        inGameWin.LeftMoveButtonEvent += MoveLeft;
        inGameWin.RightMoveButtonEvent += MoveRight;
        inGameWin.DownMoveButtonEvent += MoveDown;
        inGameWin.BackButtonEvent += () => { mUIStateManager.SwitchState((uint)UIState.MainMenu, null, null); };
        inGameWin.PauseButtonEvent += () => {
            InGameState inGameState = mUIStateManager.GetState((uint)UIState.InGame) as InGameState;
            StateManager manager = inGameState.GameStateManager;
            
            if (manager.CurrentStateID == (uint)GameState.Gaming){
                manager.SwitchState((uint)GameState.Pause, null, null);
            }
        };
        inGameWin.GrayImageEvent += () =>
        {
            InGameState inGameState = mUIStateManager.GetState((uint)UIState.InGame) as InGameState;
            StateManager manager = inGameState.GameStateManager;

            if (manager.CurrentStateID == (uint)GameState.Pause)
            {
                manager.SwitchState((uint)GameState.Gaming, null, null);
            }
        };
        inGameWin.ScoreWindowOkButtonEvent += () =>
        {
            mUIStateManager.SwitchState((uint)UIState.MainMenu, null, null);
        };
        inGameWin.CustomModeButtonEvent += () =>
        {
            mCoreGame.MGameCenter.GameMode = GameMode.CustomMode;
            InGameState inGameState = mUIStateManager.GetState((uint)UIState.InGame) as InGameState;
            inGameState.GameStateManager.SwitchState((uint)GameState.Loading, null, null);
        };
        inGameWin.TransformModeButtonEvent += () =>
        {
            mCoreGame.MGameCenter.GameMode = GameMode.TransformMode;
            InGameState inGameState = mUIStateManager.GetState((uint)UIState.InGame) as InGameState;
            inGameState.GameStateManager.SwitchState((uint)GameState.Loading, null, null);
        };
        inGameWin.TopSpeedModeButtonEvent += () =>
        {
            mCoreGame.MGameCenter.GameMode = GameMode.TopSpeedMode;
            InGameState inGameState = mUIStateManager.GetState((uint)UIState.InGame) as InGameState;
            inGameState.GameStateManager.SwitchState((uint)GameState.Loading, null, null);
        };

        GameObject storeWindow = Helper.GetPrefabTypeOfGameObject("StoreWindow");
        Helper.SetParent(storeWindow.transform, g.transform);
        storeWindow.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        mStoreWindow = storeWindow.GetComponent<StoreWindow>();
        StoreWindow storeW = mStoreWindow as StoreWindow;
        mStoreWindow.Init();
        storeW.OnGetStoreItemFailedEvent += () => { 
            mMessageBox.MIsInProgress = false;
            mMessageBox.MMessage = StoreWindow.FailedStr;
            mMessageBox.MIsShow = true;
        };
        storeW.OnGetStoreItemInProgressEvent += () =>
        {
            mMessageBox.MIsInProgress = true;
            mMessageBox.MMessage = StoreWindow.InProgressStr;
            mMessageBox.MIsShow = true;
        };
        storeW.OnGetStoreItemSuccessEvent += () =>
        {
            mMessageBox.MIsShow = false;
        };
        storeW.OnBackButtonClickEvent += () =>
        {
            mMessageBox.MIsShow = false;
            mUIStateManager.SwitchState((uint)UIState.MainMenu, null, null);
        };
        storeW.BuyItemEvent += (data) =>
        {
            if (mCoreGame.MPlayerManager.MPlayerData.itemsID.Count >= CommonSetting.playerMaxStoreItems) {
                mMessageBox.MIsInProgress = false;
                mMessageBox.MMessage = StoreWindow.OverHeightStr;
                mMessageBox.MIsShow = true;
                return;
            }
            mMessageBox.MIsInProgress = true;
            mMessageBox.MMessage = StoreWindow.BuyInProgressStr;
            mMessageBox.MIsShow = true;
            if(mCoreGame.MPlayerManager.MPlayerData.gold < int.Parse(data.price)) {
                mMessageBox.MIsShow = true;
                mMessageBox.MIsInProgress = false;
                mMessageBox.MMessage = "余额不足!"; 
                return;
            }
            User.AddStoreItemID(data.id, () => {
                mMessageBox.MIsInProgress = false;
                mMessageBox.MMessage = StoreWindow.BuySuccessStr + (mCoreGame.MPlayerManager.MPlayerData.itemsID.Count + 1).ToString() + "/" + CommonSetting.playerMaxStoreItems;
                mMessageBox.MIsShow = true;
                mCoreGame.MPlayerManager.MPlayerData.itemsID.Add(data.id);
                mCoreGame.MPlayerManager.MPlayerData.gold -= int.Parse(data.price);
                storeW.PersonalGold = mCoreGame.MPlayerManager.MPlayerData.gold.ToString();
            }, () => {
                mMessageBox.MIsInProgress = false;
                mMessageBox.MMessage = StoreWindow.BuyFailedStr;
                mMessageBox.MIsShow = true;
            });
        };
        storeW.GetPersonalGoldEvent += () => {
            storeW.PersonalGold = mCoreGame.MPlayerManager.MPlayerData.gold.ToString();
        };

        GameObject rankWindow = Helper.GetPrefabTypeOfGameObject("RankWindow");
        Helper.SetParent(rankWindow.transform, g.transform);
        rankWindow.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        mRankWindow = rankWindow.GetComponent<RankWindow>();
        RankWindow rankW = mRankWindow as RankWindow;
        mRankWindow.Init();
        rankW.OnBackButtonClickEvent += () =>
        {
            mUIStateManager.SwitchState((uint)UIState.MainMenu, null, null);
        };
        rankW.OnLoadingEvent += () =>
        {
            mMessageBox.MIsInProgress = true;
            mMessageBox.MMessage = RankWindow.LoadingStr;
            mMessageBox.MIsShow = true;
        };
        rankW.OnLoadFinishEvent += (isSuccess) =>
        {
            if (isSuccess)
            {
                mMessageBox.MIsShow = false;
            }
            else
            {
                mMessageBox.MIsInProgress = false;
                mMessageBox.MMessage = RankWindow.LoadFailedStr;
                mMessageBox.MIsShow = true;
            }
        };

        GameObject introductionWindow = Helper.GetPrefabTypeOfGameObject("IntroductionWindow");
        Helper.SetParent(introductionWindow.transform, g.transform);
        introductionWindow.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        mIntroductionWindow = introductionWindow.GetComponent<IntroductionWindow>();
        IntroductionWindow introductionW = mIntroductionWindow as IntroductionWindow;
        introductionW.Init();
        introductionW.BackButtonClickEvent += () =>
        {
            mUIStateManager.SwitchState((uint)UIState.MainMenu, null, null);
        };
        
        GameObject messageBox = Helper.GetPrefabTypeOfGameObject("MessageBox");
        Helper.SetParent(messageBox.transform, g.transform);
        mMessageBox = messageBox.GetComponent<MessageBox>();
        mMessageBox.Init();

        GameObject loadingWindow = Helper.GetPrefabTypeOfGameObject("LoadingWindow");
        Helper.SetParent(loadingWindow.transform, g.transform);
        loadingWindow.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        mLoadingWindow = loadingWindow.GetComponent<LoadingWindow>();
        mLoadingWindow.Init();

    }

    private void MoveLeft() {
        mCoreGame.MGameCenter.LeftMoveCurrentBlock();
    }

    private void MoveRight() {
        mCoreGame.MGameCenter.RightMoveCurrentBlock();
    }

    private void MoveDown() {
        mCoreGame.MGameCenter.DowmMoveCurrentBlock();
    }

    private void InitStateManager() {
        mUIStateManager = new StateManager();
        mUIStateManager.RigisterState(new StartState (this, UIState.Start));
        mUIStateManager.RigisterState(new MainMenuState (this, UIState.MainMenu));
        mUIStateManager.RigisterState(new InGameState (this, UIState.InGame));
        mUIStateManager.RigisterState(new StoreState(this, UIState.Store));
        mUIStateManager.RigisterState(new RankState(this, UIState.Rank));
        mUIStateManager.RigisterState(new IntroductionState(this, UIState.Introduction));
    }

    private void InitNotificationCenter() {
        mNotificationCenter = NotificationCenter.GetInstance();
        mNotificationCenter.AddObserver(NetWorkNotificationName.loginSuccess, DoHandler1);
    }

    private bool mIsEnterMainMenu = false;

    private void DoHandler1(Notification note) {
        mCoreGame.MPlayerManager.MPlayerData = (PlayerDataModel)note.data;
        mIsEnterMainMenu = true;
    }

}
public enum UIState { 
    Start,
    MainMenu,
    InGame,
    Store,
    Rank,
    Introduction,
}
