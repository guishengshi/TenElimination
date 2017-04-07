using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankItemUI : BaseComponent {
    public Image rankImage;
    public Text rankText;
    public Text userName;
    public Text userNickName;
    public Text maxScore;

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnHide()
    {

    }

    protected override void OnShow()
    {

    }

    protected override void RenderPerFrame(float perTime)
    {
        base.RenderPerFrame(perTime);

    }

    public void UpdateRank(int ranking) { 
        Sprite sprite = null;
        if (ranking == 1) {
            sprite = Helper.GetSprite("jin");
            rankImage.sprite = sprite;
            rankImage.gameObject.SetActive(true);
        }
        else if (ranking == 2) {
            sprite = Helper.GetSprite("yin");
            rankImage.sprite = sprite;
            rankImage.gameObject.SetActive(true);
        }
        else if (ranking == 3)
        {
            sprite = Helper.GetSprite("tong");
            rankImage.sprite = sprite;
            rankImage.gameObject.SetActive(true);
        }
        else {
            rankText.text = ranking.ToString();
            rankText.gameObject.SetActive(true);
        }
    }

    public void UpdateUI(int score, string name, string nickname) {
        userName.text = name;
        userNickName.text = nickname;
        maxScore.text = score.ToString();
    }
}
