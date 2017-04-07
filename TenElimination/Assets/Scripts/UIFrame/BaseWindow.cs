using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// 窗口基类
/// </summary>

[ExecuteInEditMode]
public class BaseWindow : BaseUI {
    public enum WindowID { 
        MainWindow,
        SecondWindow,
        ThirdWindow,
    }

    public Button[] buttons;                //处理多button相同操作的处理的集合，下面则是多button共有属性的设置

    public ButtonArrayCommonSettings settings = new ButtonArrayCommonSettings();

    private Dictionary<string, BaseUI> mChildren = new Dictionary<string, BaseUI>();        //存放每个窗口所需的组件
    private WindowID mWindowID;

    public WindowID MWindowID {
        get {
            return mWindowID;
        }
        set {
            mWindowID = value;
        }
    }

    protected override void OnInit()
    {
        uiType = BaseUIType.Window;
        if (buttons.Length == 0) {
            return;
        }
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].transition = (Selectable.Transition)settings.mode;
            if (settings.mode == ButtonArrayCommonSettings.TransitionMode.ColorTint)
            {
                ColorBlock color = new ColorBlock();
                color.normalColor = settings.normalColor;
                color.pressedColor = settings.pressedColor;
                color.highlightedColor = settings.highlightedColor;
                color.disabledColor = settings.disabledColor;
                color.colorMultiplier = 1f;
                buttons[i].colors = color;
            }
            else {
                SpriteState state = new SpriteState();
                state.disabledSprite = settings.disabledSprite; 
                state.highlightedSprite = settings.highlightedSprite;
                state.pressedSprite = settings.pressedSprite;
                buttons[i].spriteState = state;
            }
            
        }
    }

    protected override void OnHide()
    {
        foreach (KeyValuePair <string, BaseUI> kv in mChildren) {
            kv.Value.Hide();
        }
    }

    protected override void OnShow()
    {
        
    }

    protected override void RenderPerFrame(float perTime)
    {
        foreach (KeyValuePair<string, BaseUI> kv in mChildren)
        {
            if (kv.Value.Enable)
                kv.Value.UpdateUI(perTime);
        }
    }

    public void AddChild(string name, BaseUI ui) {
        if (mChildren.ContainsValue(ui)) {
            Debug.LogWarning("已经添加过该控件");
            return;
        }
        mChildren.Add(name, ui);
    }

    public BaseUI GetChild(string name) {
        
        if (mChildren.ContainsKey(name)) {
            return mChildren [name];
        }
        return null;
    }

}
