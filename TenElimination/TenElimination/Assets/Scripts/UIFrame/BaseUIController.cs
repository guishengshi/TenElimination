using UnityEngine;
using System.Collections;
/// <summary>
/// BaseUI的控制类
/// 用于对数据进行逻辑处理并传递给BaseUI
/// </summary>
public abstract class BaseUIController {
    protected BaseWindow mParentWindow;

    public BaseWindow MParentWindow {
        get {
            return mParentWindow;
        }
    }

    public BaseUIController(GameObject parent, BaseWindow parentWindow) {
        mParentWindow = parentWindow;
        Init();
        myBaseUI.Init();
        myBaseUI.Transform.SetParent(parent.transform);
        myBaseUI.Transform.localScale = Vector3.one;
        myBaseUI.Transform.localPosition = Vector3.zero;
        InitUIPosAndScale();
    }
    public BaseComponent myBaseUI;
    //此处获取对应的BaseUI
    protected abstract void Init();
    protected virtual void InitUIPosAndScale() { 
    
    }

}
