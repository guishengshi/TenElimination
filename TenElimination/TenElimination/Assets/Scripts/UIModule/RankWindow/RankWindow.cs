using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RankWindow : BaseWindow {
    public Button backButton;
    public Button customModeButton;
    public Button transformModeButton;
    public Button topSpeedModeButton;
    public Image customModeImage;
    public Image transformModeImage;
    public Image topSpeedModeImage;
    public Text myMaxScore;
    public GameObject rankItems;

    public delegate void OnBackButtonClickCall();
    public event OnBackButtonClickCall OnBackButtonClickEvent;
    public delegate void OnLoadingCall();
    public event OnLoadingCall OnLoadingEvent;
    public delegate void OnLoadFinishCall(bool isSuccess);
    public event OnLoadFinishCall OnLoadFinishEvent;

    private Sprite mCustomModeDownSprite;
    private Sprite mTransformModeDownSprite;
    private Sprite mTopSpeedModeDownSprite;
    private Sprite mCustomModeSprite;
    private Sprite mTransformModeSprite;
    private Sprite mTopSpeedModeSprite;
    private RankMode mRankMode = RankMode.Null;
    private List<RankItemController> mRankItemControllers = new List<RankItemController> ();

    private const string mLoadingStr = "加载中...";
    private const string mLoadFailedStr = "加载失败";

    public static string LoadingStr {
        get {
            return mLoadingStr;
        }
    }

    public static string LoadFailedStr {
        get {
            return mLoadFailedStr;
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        MWindowID = WindowID.MainWindow;
        InitButtonClickEvent();
        InitSprite();
        InitRankItems();
    }

    protected override void OnShow()
    {
        base.OnShow();
        CustomModeButtonOnClick();
    }

    protected override void OnHide()
    {
        base.OnHide();
        mRankMode = RankMode.Null;
    }

    private bool mIsRefreshData = false;
    private List<RankItemData> mRefreshData;

    protected override void RenderPerFrame(float perTime)
    {
        base.RenderPerFrame(perTime);
        if (mIsRefreshData) {
            for (int i = 0; i < mRankItemControllers.Count; i++)
            {
                mRankItemControllers[i].MRankItemData = mRefreshData[i];
                mRankItemControllers[i].UpdateUI();
            }
            mIsRefreshData = false;
        }
    }

    private void InitButtonClickEvent() {
        MusicPlayer player = MusicManager.GetInstance().MMusicPlayer;
        backButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            OnBackButtonClickEvent();
        });
        customModeButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            CustomModeButtonOnClick();
        });
        transformModeButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            TransformModeButtonOnClick();
        });
        topSpeedModeButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            TopSpeedModeButtonOnClick();
        });
    }

    private void InitSprite() {
        mCustomModeDownSprite = Helper.GetSprite("jingdianmoshi_down");
        mTransformModeDownSprite = Helper.GetSprite("bianhuanmoshi_down");
        mTopSpeedModeDownSprite = Helper.GetSprite("jisumoshi_down");
        mCustomModeSprite = Helper.GetSprite("jingdianmoshi");
        mTransformModeSprite = Helper.GetSprite("bianhuanmoshi");
        mTopSpeedModeSprite = Helper.GetSprite("jisumoshi");
    }

    private void CustomModeButtonOnClick() {
        if (mRankMode == RankMode.Custom)
        {
            return;
        }
        mRankMode = RankMode.Custom;
        customModeImage.sprite = mCustomModeDownSprite;
        transformModeImage.sprite = mTransformModeSprite;
        topSpeedModeImage.sprite = mTopSpeedModeSprite;
        LoadingData(User.CustomScoreStr);
    }

    private void TransformModeButtonOnClick()
    {
        if (mRankMode == RankMode.Transform)
        {
            return;
        }
        mRankMode = RankMode.Transform;
        customModeImage.sprite = mCustomModeSprite;
        transformModeImage.sprite = mTransformModeDownSprite;
        topSpeedModeImage.sprite = mTopSpeedModeSprite;
        LoadingData(User.TransformScoreStr);
    }

    private void TopSpeedModeButtonOnClick()
    {
        if (mRankMode == RankMode.TopSpeed)
        {
            return;
        }
        mRankMode = RankMode.TopSpeed;
        customModeImage.sprite = mCustomModeSprite;
        transformModeImage.sprite = mTransformModeSprite;
        topSpeedModeImage.sprite = mTopSpeedModeDownSprite;
        LoadingData(User.TopSpeedScoreStr);
    }

    private void InitRankItems() {
        for (int i = 0; i < CommonSetting.rankItemNumber; i++) {
            RankItemController controller = new RankItemController(rankItems, this, i + 1);
            controller.MRankItemData = RankItemData.Empty;
            mRankItemControllers.Add(controller);
        } 
    }

    private void LoadingData(string modeName) {
        myMaxScore.text = "我的最高分：" + User.CurrentUser[modeName].ToString();
        OnLoadingEvent();
        for (int i = 0; i < CommonSetting.rankItemNumber; i++) {
            mRankItemControllers[i].MRankItemData = RankItemData.Empty;
            mRankItemControllers[i].UpdateUI();
        }
        Quary.GetMaxScore10Users(modeName, (isSuccess, datas) => {
            if (isSuccess)
            {
                mIsRefreshData = true;
                mRefreshData = datas;
                OnLoadFinishEvent(true);
            }
            else {
                OnLoadFinishEvent(false);
            }
        });
    }

}

public enum RankMode { 
    Custom,
    Transform,
    TopSpeed,
    Null,
}