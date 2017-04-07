using UnityEngine;
using System.Collections;

public class RankItemController : BaseUIController {
    private RankItemUI mRankItemUI;
    private RankItemData mRankItemData;

    public RankItemData MRankItemData {
        get {
            return mRankItemData;
        }
        set {
            mRankItemData = value;
        }
    }

    public RankItemController(GameObject parent, BaseWindow parentWindow, int ranking)
        : base(parent, parentWindow)
    {
        mRankItemData = new RankItemData();
        mRankItemUI.UpdateRank(ranking);
    }

    protected override void Init()
    {
        GameObject g = Helper.GetPrefabTypeOfGameObject("RankItem") as GameObject;
        myBaseUI = g.GetComponent<RankItemUI>();
        mRankItemUI = myBaseUI as RankItemUI;
    }

    public void UpdateUI() {
        mRankItemUI.UpdateUI(mRankItemData.userMaxScore, mRankItemData.userName, mRankItemData.userNickName);
    }
}
