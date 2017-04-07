using UnityEngine;
using System.Collections;

/// <summary>
/// UI基类
/// </summary>
public abstract class BaseUI : MonoBehaviour {
    public enum BaseUIType { 
        Window,
        UI,
    }
    private Transform mTransform;
    private GameObject mGameObject;
    private bool mEnabled = false;
    protected BaseUIType uiType;
    protected string uiName;

    public BaseUIType UIType {
        get { return uiType; }
    }

    public bool Enable {
        get {
            return mEnabled;
        }
        set {
            if (value != mEnabled)
            {
                mGameObject.SetActive(value);
                mEnabled = value;
            }
        }
    }

    public string UIName {
        get {
            return uiName;
        }
    }

    public Transform Transform
    {
        get { return mTransform; }
        set { mTransform = value; }
    }
    public GameObject GameObject
    {
        get { return mGameObject; }
        set { mGameObject = value; }
    }
    public void Init() {
        mTransform = transform;
        mGameObject = gameObject;
        OnInit();
    }
    protected abstract void OnInit();
    public void Show() {
        mGameObject.SetActive(true);
        mEnabled = true;
        OnShow();
    }
    public void Hide() {
        mGameObject.SetActive(false);
        mEnabled = false;
        OnHide();
    }
    protected abstract void OnHide();
    protected abstract void OnShow();
    protected virtual void OnFocus() { }
    protected virtual void DisFocus() { }
    public void UpdateUI(float perTime) {
        if (Enable)
            RenderPerFrame(perTime);
    }
    protected virtual void RenderPerFrame(float perTime) { }
}
