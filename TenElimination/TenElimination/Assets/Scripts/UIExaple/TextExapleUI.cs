using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextExapleUI : BaseUI {
    public float time = 0f;  //几秒换一次颜色
    private Text mContent;
    public string MContent {
        get {
            return mContent.text;
        }
        set {
            mContent.text = value;
        }
    }
    protected override void OnInit()
    {
        mContent = GetComponent<Text>();
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
         
    }
    private float mTimer = 0f;
    protected override void RenderPerFrame(float perTime)
    {
        if (mTimer >= time)
        {
            mTimer = 0f;
        }
        else {
            mContent.color = Color.Lerp(Color.blue, Color.green, mTimer / time);
            mTimer += perTime;
        }
    }
}
