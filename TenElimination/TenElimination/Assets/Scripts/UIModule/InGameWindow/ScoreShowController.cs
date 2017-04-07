using UnityEngine;
using System.Collections;
/// <summary>
/// 分数显示的控制类
/// </summary>

public class ScoreShowController : BaseUIController {
    private ScoreShowUI mScoreShowUI;
    private ScoreShowModel mScoreShowShowModel;

    public ScoreShowUI MScoreShowUI
    {
        get { return mScoreShowUI; }
        set { mScoreShowUI = value; }
    }

    public ScoreShowController(GameObject parent, BaseWindow parentWindow) : base (parent, parentWindow) {
        mScoreShowShowModel = new ScoreShowModel(); 
    }

    protected override void Init()
    {
        GameObject g = Helper.GetPrefabTypeOfGameObject("ScoreShow") as GameObject;
        myBaseUI = g.GetComponent<ScoreShowUI>();
        mScoreShowUI = myBaseUI as ScoreShowUI;
    }

    protected override void InitUIPosAndScale()
    {
        base.InitUIPosAndScale();
        mScoreShowUI.GetComponent<RectTransform> ().anchoredPosition = Vector3.zero;
    }

    public void UpdateScore(int score) {
        mScoreShowShowModel.score = score;
        mScoreShowUI.UpdateScoreShow(mScoreShowShowModel.score);
    }

    public void AddScore(int score) {
        mScoreShowShowModel.score += score;
        mScoreShowUI.UpdateScoreShow(mScoreShowShowModel.score);
    }
}
