using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// 起始窗口
/// </summary>

public class StartWindow : BaseWindow {
    public Button loginButton;
    public Button registerButton;
    public AudioSource bgMusic;

    private RegisterWindow mRegisterWindow;
    private LoginWindow mLoginWindow;

    public RegisterWindow MRegisterWindow
    {
        get { return mRegisterWindow; }
    }

    public LoginWindow MLoginWindow {
        get {
            return mLoginWindow;
        }
    }
    
    protected override void OnInit()
    {
        base.OnInit();
        MWindowID = WindowID.MainWindow;
        InitSecondWindow();
        InitComponent();
    }

    private void InitComponent() {
        MusicPlayer player = MusicManager.GetInstance().MMusicPlayer;
        loginButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            mLoginWindow.Enable = !mLoginWindow.Enable;
            if (mRegisterWindow.Enable) {
                mRegisterWindow.Hide();
            }
        });
        registerButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            mRegisterWindow.Enable = !mRegisterWindow.Enable;
            if (mLoginWindow.Enable) {
                mLoginWindow.Hide();
            } 
        });
    }

    private void InitSecondWindow() {
        GameObject registerWindow = Helper.GetPrefabTypeOfGameObject("RegisterWindow");
        Helper.SetParent(registerWindow.transform, Transform);
        //registerWindow.transform.localPosition = new Vector3(540f, 890f, 0);
        mRegisterWindow = registerWindow.GetComponent<RegisterWindow>();
        mRegisterWindow.Init();
        GameObject loginWindow = Helper.GetPrefabTypeOfGameObject("LoginWindow");
        Helper.SetParent(loginWindow.transform, Transform);
        //loginWindow.transform.localPosition = new Vector3(540f, 850f, 0);
        mLoginWindow = loginWindow.GetComponent<LoginWindow>();
        mLoginWindow.Init();
        AddChild(mLoginWindow.UIName, mLoginWindow);
        AddChild(mRegisterWindow.UIName, mRegisterWindow);
    }
}
