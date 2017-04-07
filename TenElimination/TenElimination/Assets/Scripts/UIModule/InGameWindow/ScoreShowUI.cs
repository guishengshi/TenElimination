using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 分数显示的UI类
/// </summary>

public class ScoreShowUI : BaseComponent {
    public Text mScoreText;

    protected override void OnInit()
    {
        base.OnInit();
        mScoreText.text = "0";
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        mScoreText.text = "0";
    }

    protected override void RenderPerFrame(float perTime)
    {
        base.RenderPerFrame(perTime);

    }

    public void UpdateScoreShow(int score) {
        mScoreText.text = score.ToString();
    }
}
