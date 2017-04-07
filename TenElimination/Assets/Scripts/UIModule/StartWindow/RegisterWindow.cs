using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AVOSCloud;

public class RegisterWindow : BaseWindow {
    public Button okButton;
    public Button backButton;
    public InputField accountInput;
    public InputField nickNameInput;
    public InputField passwordInput;
    public InputField passwordAgainInput;
    public Image accountEnableImage;
    public Image nickNameEnableImage;
    public Image passwordEnableImage;
    public Image passwordAgainEnableImage;

    private Sprite mEnableSprite;
    private Sprite mDisableSprite;
    private bool mIsAccountUsed;
    private bool mIsNickNameUsed;
    private bool mIsPasswordUsed;
    private bool mIsPasswordAgainUsed;
    private bool mIsAccountShow = false;
    private bool mIsNickNameShow = false;
    public const string mRegisterSuccessStr = "注册成功";
    public const string mRegisterFailedStr = "注册失败";
    public const string mRegisterInProgressStr = "注册中";

    public delegate void OnRegisterSuccessCall();
    public event OnRegisterSuccessCall OnRegisterSuccessEvent;
    public delegate void OnRegisterFailedCall();
    public event OnRegisterFailedCall OnRegisterFailedEvent;
    public delegate void OnRegisterInProgressCall();
    public event OnRegisterInProgressCall OnRegisterInProgressEvent;

    protected override void OnInit()
    {
        base.OnInit();
        MWindowID = WindowID.SecondWindow;
        uiName = "RegisterWindow";
        InitComponent();
    }

    protected override void OnShow()
    {
        base.OnShow();
    }

    protected override void OnHide()
    {
        base.OnHide();
        accountInput.text = "";
        nickNameInput.text = "";
        passwordInput.text = "";
        passwordAgainInput.text = "";
    }

    private bool mIsRegisterSuccess;

    protected override void RenderPerFrame(float perTime)
    {
        base.RenderPerFrame(perTime);
        if (mIsAccountUsed)
        {
            accountEnableImage.sprite = mEnableSprite;
        }
        else {
            accountEnableImage.sprite = mDisableSprite;
        }
        if (mIsNickNameUsed)
        {
            nickNameEnableImage.sprite = mEnableSprite;
        }
        else {
            nickNameEnableImage.sprite = mDisableSprite;
        }
        if (mIsAccountShow)
        {
            accountEnableImage.gameObject.SetActive(true);
        }
        else {
            accountEnableImage.gameObject.SetActive(false);
        }
        if (mIsNickNameShow)
        {
            nickNameEnableImage.gameObject.SetActive(true);
        }
        else {
            nickNameEnableImage.gameObject.SetActive(false);
        }
        if (mIsRegisterSuccess) {
            Hide();
            mIsRegisterSuccess = false;
        }
    }

    private void InitComponent() {
        MusicPlayer player = MusicManager.GetInstance().MMusicPlayer;
        backButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            Hide();
        });
        mEnableSprite = Helper.GetSprite("yes");
        mDisableSprite = Helper.GetSprite("no");
        okButton.onClick.AddListener(delegate() {
            player.PlayButtonMusic();
            Register();
        });
        accountInput.onValueChange.AddListener(delegate(string text) {
                mIsAccountUsed = false;
                mIsAccountShow = false;
        });
        nickNameInput.onValueChange.AddListener(delegate(string text)
        {
                mIsNickNameUsed = false;
                mIsNickNameShow = false;
        });
        passwordInput.onValueChange.AddListener(delegate(string text)
        {
                mIsPasswordUsed = false;
                passwordEnableImage.gameObject.SetActive(false);
        });
        passwordAgainInput.onValueChange.AddListener(delegate(string text)
        {
                mIsPasswordAgainUsed = false;
                passwordAgainEnableImage.gameObject.SetActive(false);
        });
        accountInput.onEndEdit.AddListener(delegate(string text) {
            if (string.IsNullOrEmpty(text)) {
                return;
            }
            if (!CheckCharacterLimit(text, 9, 16)) {
                mIsAccountUsed = false;
                mIsAccountShow = true;
                return;
            }
            Quary.VerifyUserName(text, (bool isSuccess) => {
                if (isSuccess) {
                    mIsAccountUsed = true;
                    mIsAccountShow = true;
                } else {
                    mIsAccountUsed = false;
                    mIsAccountShow = true;
                    //accountEnableImage.sprite = mDisableSprite;
                }
            });
            //if (CheckCharacterLimit(text, 9, 16))
            //{
            //    mIsAccountUsed = true;
            //    accountEnableImage.sprite = mEnableSprite;
            //}
            //accountEnableImage.gameObject.SetActive(true);
        });
        nickNameInput.onEndEdit.AddListener(delegate(string text) {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (!CheckCharacterLimit(text, 0, 9))
            {
                mIsNickNameUsed = false;
                mIsNickNameShow = true;
            }
            Quary.VerifyNickName(text, (bool isSuccess) => {
                if (isSuccess) {
                    mIsNickNameUsed = true;
                    mIsNickNameShow = true;
                } else {
                    mIsNickNameUsed = false;
                    mIsNickNameShow = true;
                }
            });
            //else
            //{
            //    mIsNickNameUsed = false;
            //    nickNameEnableImage.sprite = mDisableSprite;
            //}
            //nickNameEnableImage.gameObject.SetActive(true);
        });
        passwordInput.onEndEdit.AddListener(delegate(string text) {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (CheckCharacterLimit(text, 9, 16))
            {
                mIsPasswordUsed = true;
                passwordEnableImage.sprite = mEnableSprite;
            }
            else {
                mIsPasswordUsed = false;
                passwordEnableImage.sprite = mDisableSprite;
            }
            passwordEnableImage.gameObject.SetActive(true);
        });
        passwordAgainInput.onEndEdit.AddListener(delegate(string text) {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (text.Equals(passwordInput.text))
            {
                mIsPasswordAgainUsed = true;
                passwordAgainEnableImage.sprite = mEnableSprite;
            }
            else {
                mIsPasswordAgainUsed = false;
                passwordAgainEnableImage.sprite = mDisableSprite;
            }
            passwordAgainEnableImage.gameObject.SetActive(true);
        });
    }

    private bool CheckUserName(string text) {
        
        return true;
    }

    private bool CheckCharacterLimit(string text, int min ,int max) {
        if (text.Length < min || text.Length > max) {
            return false;
        }
        return true;
    }

    private void Register() {
        if (!mIsAccountUsed || !mIsNickNameUsed || !mIsPasswordUsed || !mIsPasswordAgainUsed)
        {
            return;
        }
        User user = new User(accountInput.text, nickNameInput.text, passwordInput.text);
        user.Register(OnRegisterFailed, OnRegisterSuccess);
        OnRegisterInProgressEvent();
        
    }

    private void OnRegisterSuccess() {
        OnRegisterSuccessEvent();
        mIsRegisterSuccess = true;
    }

    private void OnRegisterFailed() {
        OnRegisterFailedEvent();
        
    }
}
