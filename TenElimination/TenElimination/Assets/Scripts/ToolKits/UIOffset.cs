using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// 用于UI适配的工具类，需要UGUI的Canvas
/// </summary>

[ExecuteInEditMode]
[RequireComponent (typeof (CanvasScaler))]
public class UIOffset : MonoBehaviour {
    public bool enable = true;
    public bool isUpdate = true;
    public bool isActive = true;

    //private GameObject mGameObject;
    //private Transform mTransform;
    private CanvasScaler mCanvasScaler;

    private float mOriginalX;
    private float mOriginalY;

    void Awake() {
        //mGameObject = gameObject;
        //mTransform = transform;
        mCanvasScaler = transform.GetComponent <CanvasScaler> ();
        mOriginalX = mCanvasScaler.referenceResolution.x;
        mOriginalY = mCanvasScaler.referenceResolution.y;
    }

	void Start () {
        if (enable) {
            Refresh();
        }
	}
	
	void Update () {
        if (isUpdate && enable) {
            Refresh();
        }
	}

    void OnEnable() {
        if (isActive && enable) {
            Refresh();
        }
    }

    private void Refresh() {
        float xScale = Screen.width / mOriginalX;
        float yScale = Screen.height / mOriginalY;
        if (xScale < yScale)
        {
            mCanvasScaler.matchWidthOrHeight = 0;
        }
        else {
            mCanvasScaler.matchWidthOrHeight = 1;
        }
    }
}
