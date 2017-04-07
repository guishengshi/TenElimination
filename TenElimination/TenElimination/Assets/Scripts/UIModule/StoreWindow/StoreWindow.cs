using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StoreWindow : BaseWindow {
    public Button backButton;
    public GameObject storeItems;
    public Text myGoldText;

    private const string mInProgressStr = "加载中...";
    private const string mFailedStr = "连接超时";
    private const string mBuySuccessStr = "购买成功！";
    private const string mBuyFailedStr = "购买失败！";
    private const string mBuyInProgressStr = "购买中...";
    private const string mItemOverWeightStr = "物品携带已达上限";
    private List<StoreItemData> mStoreItemDatas;
    private string mPersonalGold;

    public delegate void OnGetStoreItemFailedCall();
    public delegate void OnGetStoreItemInProgressCall();
    public delegate void OnGetStoreItemSuccessCall();
    public event OnGetStoreItemFailedCall OnGetStoreItemFailedEvent;
    public event OnGetStoreItemInProgressCall OnGetStoreItemInProgressEvent;
    public event OnGetStoreItemSuccessCall OnGetStoreItemSuccessEvent;

    public delegate void OnBackButtonClickCall();
    public event OnBackButtonClickCall OnBackButtonClickEvent;

    public delegate void BuyItemCall(StoreItemData data);
    public event BuyItemCall BuyItemEvent;
    public delegate void GetPersonalGoldCall();
    public event GetPersonalGoldCall GetPersonalGoldEvent;

    public static string InProgressStr {
        get {
            return mInProgressStr;
        }
    }

    public static string FailedStr {
        get {
            return mFailedStr;
        }
    }

    public static string BuySuccessStr {
        get {
            return mBuySuccessStr;
        }
    }

    public static string BuyFailedStr {
        get {
            return mBuyFailedStr;
        }
    }

    public static string BuyInProgressStr {
        get {
            return mBuyInProgressStr;
        }
    }

    public static string OverHeightStr {
        get {
            return mItemOverWeightStr;
        }
    }

    public string PersonalGold {
        get {
            return mPersonalGold;
        }
        set {
            mPersonalGold = value;
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        MWindowID = WindowID.MainWindow;
        InitButtonClickEvent();
        //mScoreShowController = new ScoreShowController(mBottomBG, this);
        //mScoreShowController.MScoreShowUI.Show();
    }

    protected override void OnShow()
    {
        base.OnShow();
        if (isInitStoreItems) {
            return;
        }
        GetPersonalGoldEvent();
        OnGetStoreItemInProgressEvent();
        Quary.GetAllStoreItems((bool isSuccess, List <StoreItemData> items) => {
            if (isSuccess)
            {
                mStoreItemDatas = items;
                isInitStoreItems = true;
                OnGetStoreItemSuccessEvent();
            }
            else {
                OnGetStoreItemFailedEvent();
            }
        });
    }

    private bool isInitStoreItems = false;
    private bool isInitStoreItemsCompleted = false;

    protected override void RenderPerFrame(float perTime)
    {
        base.RenderPerFrame(perTime);
        myGoldText.text = mPersonalGold;
        if (isInitStoreItemsCompleted) {
            return;
        }
        if (isInitStoreItems) {
            InitStoreItems();         
        }
    }
   
    private void InitButtonClickEvent() {
        MusicPlayer player = MusicManager.GetInstance().MMusicPlayer;
        backButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            OnBackButtonClickEvent();
        });
    }

    private void InitStoreItems() {
        GridLayoutGroup grid = storeItems.GetComponent<GridLayoutGroup>();
        RectTransform rect = storeItems.GetComponent<RectTransform>();
        float height = grid.cellSize.y + grid.spacing.y;
        height = (mStoreItemDatas.Count + 1) / 2 * height;
        rect.localPosition = new Vector3(0, -height / 2, 0);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        //rect.sizeDelta = new Vector2(936f, height);
        for (int i = 0; i < mStoreItemDatas.Count; i++) {
            StoreItemController controller = new StoreItemController(storeItems, this, mStoreItemDatas [i]);
            controller.BuyButtonEvent += (data) =>
            {
                BuyItemEvent(data);
            };
            controller.UpdateUI();
        }
        isInitStoreItemsCompleted = true;
        
    }

    
}
