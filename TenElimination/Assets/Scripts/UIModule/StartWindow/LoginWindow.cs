using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AVOSCloud;

public class LoginWindow : BaseWindow {
    public Button okButton;
    public Button backButton;
    public InputField accountInput;
    public InputField passwordInput;

    public const string OnLoginSuccessStr = "登陆成功";
    public const string OnLoginFailedStr = "登录失败";
    public const string OnLoginInProgressStr = "登录中...";

    public delegate void OnLoginSuccessCall();
    public event OnLoginSuccessCall OnLoginSuccessEvent;
    public delegate void OnLoginFailedCall();
    public event OnLoginFailedCall OnLoginFailedEvent;
    public delegate void OnLoginInProgressCall();
    public event OnLoginInProgressCall OnLoginInProgressEvent;

    //public delegate void OnLoginSuccessCall(PlayerDataModel model);
    //public event OnLoginSuccessCall OnLoginSuccessEvent;
    //public delegate void OnLoginFailCall();
    //public event OnLoginFailCall OnLoginFailEvent;

    protected override void OnInit()
    {
        base.OnInit();
        MWindowID = WindowID.SecondWindow;
        uiName = "LoginWindow";
        InitComponent();
    }

    protected override void OnHide()
    {
        base.OnHide();
        accountInput.text = "";
        passwordInput.text = "";
    }

    protected override void OnShow()
    {
        base.OnShow();
    }

    private void InitComponent() {
        MusicPlayer player = MusicManager.GetInstance().MMusicPlayer;
        backButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            Hide();
        });
        okButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            Login();
        });
    }

    private void Login() {
        User.Login(accountInput.text, passwordInput.text, OnLoginFailed, OnLoginSuccess);
        OnLoginInProgressEvent();
    }

    private void OnLoginSuccess(PlayerDataModel model) {
        OnLoginSuccessEvent();
        NotificationCenter.GetInstance().PostNotification(NetWorkNotificationName.loginSuccess, this, model);
    }

    private void OnLoginFailed() {
        OnLoginFailedEvent();
    }

}
