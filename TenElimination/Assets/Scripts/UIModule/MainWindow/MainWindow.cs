using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 主窗口
/// </summary>
public class MainWindow : BaseWindow {
    public Button mStartGameButton;
    public Button mRankButton;
    public Button mGameIntroductionButton;
    public Button mStoreButton;
    public Button mGameOverButton;
    public AudioSource bgMusic;

    public delegate void StartGameDelegate();
    public StartGameDelegate StartGameCall;
    public delegate void RankDelegate();
    public RankDelegate RankCall;
    public delegate void GameIntroductionDelegate();
    public GameIntroductionDelegate GameIntroductionCall;
    public delegate void StoreDelegate();
    public StoreDelegate StoreCall;
    public delegate void GameOverDelegate();
    public GameOverDelegate GameOverCall;


    protected override void OnInit()
    {
        base.OnInit();
        MWindowID = BaseWindow.WindowID.MainWindow;
        InitButtonClickEvent();
    }

    private void InitButtonClickEvent() {
        MusicPlayer player = MusicManager.GetInstance().MMusicPlayer;
        mStartGameButton.onClick.AddListener(delegate() { 
            StartGameCall();
            player.PlayButtonMusic();
        });
        mRankButton.onClick.AddListener(delegate () { 
            RankCall();
            player.PlayButtonMusic();
        });
        mGameIntroductionButton.onClick.AddListener(delegate { 
            GameIntroductionCall();
            player.PlayButtonMusic();
        });
        mStoreButton.onClick.AddListener(delegate { 
            StoreCall();
            player.PlayButtonMusic();
        });
        mGameOverButton.onClick.AddListener(delegate { 
            GameOverCall();
            player.PlayButtonMusic();
        });
    }
}
