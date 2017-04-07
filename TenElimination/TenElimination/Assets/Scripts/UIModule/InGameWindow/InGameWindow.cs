using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InGameWindow : BaseWindow {
    public Button mLeftMoveButton;
    public Button mRightMoveButton;
    public Button mDownMoveButton;
    public Button mBackButton;
    public Button mPauseButton;
    public Button mCustomModeButton;
    public Button mTransformModeButton;
    public Button mTopSpeedModeButton;
    public GameObject mModeSelectWindow;
    public Image mLeftMoveImage;
    public Image mRightMoveImage;
    public Image mDownMoveImage;
    public Image mPointerImage;
    public EventTrigger mGrayImage;
    public Text mTimeShow;
    public GameObject mBottomBG;
    public float imageShowTime = 1f;
    public RawImage[] skillsImage;
    public EventTrigger[] skillButton;
    public GameObject scoreWindow;
    public Text currentScoreResultText;
    public Text maxScoreText;
    public Button scoreWindowOkButton;
    public Image maskImage;
    public Text goldText;
    public AudioSource bgMusicSource;
    public AudioClip customModeMusic;
    public AudioClip transformModeMusic;
    public AudioClip topSpeedModeMusic;

    public delegate void LeftMoveButtonCall();
    public delegate void RightMoveButtonCall();
    public delegate void DownMoveButtonCall();
    public delegate void BackButtonCall();
    public delegate void PauseButtonCall();
    public delegate void GrayImageCall();
    public delegate void ScoreWindowOkButtonCall();
    public delegate void CustomModeButtonCall();
    public delegate void TransformModeButtonCall();
    public delegate void TopSpeedModeButtonCall();
    public event LeftMoveButtonCall LeftMoveButtonEvent;
    public event RightMoveButtonCall RightMoveButtonEvent;
    public event DownMoveButtonCall DownMoveButtonEvent;
    public event BackButtonCall BackButtonEvent;
    public event PauseButtonCall PauseButtonEvent;
    public event GrayImageCall GrayImageEvent;
    public event ScoreWindowOkButtonCall ScoreWindowOkButtonEvent;
    public event CustomModeButtonCall CustomModeButtonEvent;
    public event TransformModeButtonCall TransformModeButtonEvent;
    public event TopSpeedModeButtonCall TopSpeedModeButtonEvent;
    

    private ScoreShowController mScoreShowController;

    public ScoreShowController MScoreShowController {
        get {
            return mScoreShowController;
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        MWindowID = WindowID.MainWindow;
        InitButtonClickEvent();
        mScoreShowController = new ScoreShowController(mBottomBG, this);
        mScoreShowController.MScoreShowUI.Show();
    }

    protected override void OnHide()
    {
        base.OnHide();
        mLeftMoveImage.gameObject.SetActive(false);
        mRightMoveImage.gameObject.SetActive(false);
        mDownMoveImage.gameObject.SetActive(false);
    }

    private void InitButtonClickEvent() {
        LeftMoveButtonEvent += LeftMoveImageControl;
        RightMoveButtonEvent += RightMoveImageControl;
        DownMoveButtonEvent += DownMoveImageControl;
        mLeftMoveButton.onClick.AddListener(delegate() { LeftMoveButtonEvent(); });
        mRightMoveButton.onClick.AddListener(delegate() { RightMoveButtonEvent(); });
        mDownMoveButton.onClick.AddListener(delegate() { DownMoveButtonEvent(); });

        mBackButton.onClick.AddListener(delegate() { BackButtonEvent(); });
        mPauseButton.onClick.AddListener(delegate() { PauseButtonEvent(); });

        mGrayImage.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((arg0) => {
            GrayImageEvent();
        });
        mGrayImage.triggers.Add(entry);

        scoreWindowOkButton.onClick.AddListener(delegate() {
            ScoreWindowOkButtonEvent();        
        });

        mCustomModeButton.onClick.AddListener(delegate() {
            CustomModeButtonEvent();
        });
        mTransformModeButton.onClick.AddListener(delegate()
        {
            TransformModeButtonEvent();
        });
        mTopSpeedModeButton.onClick.AddListener(delegate()
        {
            TopSpeedModeButtonEvent();
        });
    }


    private float mLeftMoveImageShowTimer = 0f;
    private float mRightMoveImageShowTimer = 0f;
    private float mDownMoveImageShowTimer = 0f;

    private void LeftMoveImageControl () {
        if (!mLeftMoveImage.gameObject.activeSelf) {
            mLeftMoveImage.gameObject.SetActive (true);
            StartCoroutine (LeftMoveImageShow ());
        } else {
            mLeftMoveImageShowTimer = 0f;
        }
    }

    private void RightMoveImageControl () {
        if (!mRightMoveImage.gameObject.activeSelf) {
            mRightMoveImage.gameObject.SetActive (true);
            StartCoroutine(RightMoveImageMove());
        } else {
            mRightMoveImageShowTimer = 0f;
        }
    }

    private void DownMoveImageControl () {
        if (!mDownMoveImage.gameObject.activeSelf) {
            mDownMoveImage.gameObject.SetActive (true);
            StartCoroutine (DowmMoveImageMove());
        } else {
            mDownMoveImageShowTimer = 0f;
        }
    }

    IEnumerator LeftMoveImageShow() { 
        while (mLeftMoveImageShowTimer <= imageShowTime) {
            mLeftMoveImageShowTimer += Time.deltaTime;
            yield return null;
        }

        mLeftMoveImageShowTimer = 0f;
        mLeftMoveImage.gameObject.SetActive (false);
    }

    IEnumerator RightMoveImageMove () {
        while (mRightMoveImageShowTimer <= imageShowTime) {
            mRightMoveImageShowTimer += Time.deltaTime;
            yield return null;
        }
        mRightMoveImageShowTimer = 0f;
        mRightMoveImage.gameObject.SetActive (false);
    }

    IEnumerator DowmMoveImageMove () {
        while (mDownMoveImageShowTimer <= imageShowTime) {
            mDownMoveImageShowTimer += Time.deltaTime;
            yield return null;
        }
        mDownMoveImage.gameObject.SetActive (false);
        mDownMoveImageShowTimer = 0f;
    }

    public void SetSkill(List <BaseSkill> skills) {
        if (skills.Count > 4 || skills.Count <= 0) {
            return;
        }
        for (int i = 0; i < skills.Count; i++) {
            StartCoroutine(LoadingImage(skillsImage[i], skills[i].MStoreItemData.itemImageURL));
            skillButton[i].gameObject.SetActive(true);
            skillButton[i].triggers = new System.Collections.Generic.List<EventTrigger.Entry>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            BaseSkill skill = skills[i];
            RawImage image = skillsImage[i];
            EventTrigger button = skillButton[i];
            entry.callback.AddListener((arg0) =>
            {
                skill.Play();
                Tween.EnLarge(this, image.transform, 1f, Vector3.one, Vector3.one * 1.2f, () => {
                    image.transform.localScale = Vector3.one;
                    button.gameObject.SetActive(false);
                });
            });
            skillButton[i].triggers.Add(entry);
        }
    }

    IEnumerator LoadingImage(RawImage image, string url) {
        WWW w = new WWW(url);
        while (!w.isDone) {
            yield return null;
        }
        image.texture = w.texture;
    }
}
