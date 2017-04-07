using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroductionWindow : BaseWindow {
    public Button backButton;

    public delegate void BackButtonClickCall();
    public event BackButtonClickCall BackButtonClickEvent;

    protected override void OnInit()
    {
        base.OnInit();
        MWindowID = WindowID.MainWindow;
        InitButtonClickEvent();
        //mScoreShowController = new ScoreShowController(mBottomBG, this);
        //mScoreShowController.MScoreShowUI.Show();
    }

    private void InitButtonClickEvent() {
        backButton.onClick.AddListener(delegate() {
            MusicManager.GetInstance().MMusicPlayer.PlayButtonMusic();
            BackButtonClickEvent();
        });
    }

}
