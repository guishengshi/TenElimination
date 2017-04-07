using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 共有的消息通知框
/// </summary>

public class MessageBox : MonoBehaviour {
    public Text messageText;
    public Image progressImage;
    public float showTime = 1f;

    private string mMessage;
    private bool mIsInProgress = false;
    private bool mIsShow = false;
    private GameObject mGameObject;
    //private Transform mTransform;

    public bool MIsInProgress
    {
        get { return mIsInProgress; }
        set { mIsInProgress = value; }
    }

    public bool MIsShow {
        get {
            return mIsShow;
        }
        set {
            mIsShow = value;
        }
    }

    public string MMessage
    {
        get { return mMessage; }
        set { 
            mMessage = value;
        }
    }

	void Start () {
        
	}
	
	public void UpdateMessageBox (float timeDelta) {
        if (mIsShow) {
            Show();
            MessageTextController();
            ProgressController();
        } else {
            mGameObject.SetActive(false);
        }
	}

    public void Init() {
        mGameObject = gameObject;
    }

    public void Show() {
        if (!mGameObject.activeSelf) {
            mGameObject.SetActive(true);
        }
        
        if (!mIsInProgress)
        {
            if (progressImage.gameObject.activeSelf)
            {
                StartCoroutine(ShowInSeconds());
                progressImage.gameObject.SetActive(false);
            }
        }
        else {
            if (!progressImage.gameObject.activeSelf)
                progressImage.gameObject.SetActive(true);
        }
    }

    private void MessageTextController() {
        messageText.text = mMessage;
    }

    private void ProgressController() {
        if (mIsInProgress)
        {
            progressImage.transform.Rotate(progressImage.transform.forward, -10f);
        }

    }

    IEnumerator ShowInSeconds() {
        yield return new WaitForSeconds(showTime);
        mGameObject.SetActive(false);
        mIsShow = false;
        progressImage.gameObject.SetActive(true);
    }
}
