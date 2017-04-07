using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour {
    public Slider mSlider;
    private float mTime;
    private GameObject mGameObject;
    private float mCounter = 0f;

    public delegate void OnFinishedCall();
    public OnFinishedCall mOnFinishedCall;

    public void Init() {
        mGameObject = gameObject;
    }

    public void Show(float time, OnFinishedCall call) {
        mTime = time;
        mOnFinishedCall = call;
        mGameObject.SetActive(true);
    }

    public void UpdateLoadingWindow(float timeDelta) {
        if (mCounter >= mTime)
        {
            mGameObject.SetActive(false);
        }
        else {
            mCounter += timeDelta;
            mSlider.value = mCounter / mTime;
        }
    }

    public void OnEnable() {
        mCounter = 0f;
        mSlider.value = 0f;
    }

    public void OnDisable() {
        mOnFinishedCall();
    }
}
