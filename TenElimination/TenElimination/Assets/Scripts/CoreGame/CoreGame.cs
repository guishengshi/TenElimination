using UnityEngine;
using System.Collections;
/// <summary>
/// 脚本功能：全局管理器，存储各模块的引用。
/// </summary>

public abstract class CoreGame : ICommonState {
    private UIManager mUIManager;
    private GameCenter mGameCenter;
    private PlayerManager mPlayerManager;
    private NetWork mNetWork;
    private MusicManager mMusicManager;

    public GameCenter MGameCenter
    {
        get { return mGameCenter; }
        //set { mGameCenter = value; }
    }


    public UIManager MUIManager
    {
        get { return mUIManager; }
        //set { mUIManager = value; }
    }

    public PlayerManager MPlayerManager {
        get {
            return mPlayerManager;
        }
    }

    public MusicManager MMusicManager {
        get {
            return mMusicManager;
        }
    }

    protected virtual void Init() {
        mNetWork = new NetWork(this);
        mNetWork.Awake();
        mUIManager = new UIManager(this);
        mUIManager.Awake();
        mGameCenter = new GameCenter(this);
        mGameCenter.Init();
        mPlayerManager = new PlayerManager(this);
        mMusicManager = MusicManager.GetInstance();
    }

    public void Awake()
    {
        Init();
    }

    public void Start()
    {
        mUIManager.Start();
    }

    public void Update(float timeDelta)
    {
        mUIManager.Update(timeDelta);
    }

    public void LateUpdate(float timeDelta)
    {
        
    }

    public void FixedUpdate(float timeDelta)
    {
        
    }

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        
    }
}
