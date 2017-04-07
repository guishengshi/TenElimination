using UnityEngine;
using System.Collections;

/// <summary>
/// 采用扩大背景图的策略避免宽频手机屏幕出现空白
/// </summary>

[ExecuteInEditMode]
public class CameraAdaptation : MonoBehaviour {
    public GameObject bgImage;
    public float defaultWidth;
    public float defaultHeight;
    private float mCurrentWidth = Screen.width;
    private float mCurrentHeight = Screen.height;
    private float mCurrentAspectRatio;

    void Start() {
        Adapt();
    }

    void Update() {
        Adapt();
    }

    private void Adapt() {
        mCurrentAspectRatio = mCurrentWidth / mCurrentHeight;
        float defaultRatio = defaultWidth / defaultHeight;
        if (mCurrentAspectRatio >= defaultRatio) {
            bgImage.transform.localScale = new Vector3(mCurrentAspectRatio / defaultRatio, 1, 1);
        }
    }
}
