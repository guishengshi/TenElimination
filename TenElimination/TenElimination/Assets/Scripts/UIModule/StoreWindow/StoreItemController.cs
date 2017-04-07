using UnityEngine;
using System.Collections;

public class StoreItemController : BaseUIController {
    private StoreItemUI mStoreItemUI;
    private StoreItemData mStoreItemData;

    public delegate void BuyButtonCall(StoreItemData data);
    public event BuyButtonCall BuyButtonEvent;

    public StoreItemController(GameObject parent, BaseWindow parentWindow, StoreItemData data)
        : base(parent, parentWindow)
    {
        mStoreItemData = data;
    }

    protected override void Init()
    {
        GameObject g = Helper.GetPrefabTypeOfGameObject("StoreItem") as GameObject;
        myBaseUI = g.GetComponent<StoreItemUI>();
        mStoreItemUI = myBaseUI as StoreItemUI;
        MusicPlayer player = MusicManager.GetInstance().MMusicPlayer;
        mStoreItemUI.buyButton.onClick.AddListener(() => { 
            BuyButtonEvent(mStoreItemData);
            player.PlayButtonMusic();
        });
    }

    public void UpdateUI() {
        mStoreItemUI.UpdateUI(mStoreItemData.itemImageURL, mStoreItemData.itemContent, mStoreItemData.price);
    }


}
