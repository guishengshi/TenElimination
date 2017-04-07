using UnityEngine;
using System.Collections;
/// <summary>
/// 组件的基类
/// </summary>
public class BaseComponent : BaseUI {
    protected BaseWindow parentWindow;
    public BaseWindow ParentWindow {
        get {
            return parentWindow;
        }
        set {
            parentWindow = value;
        }
    }

    protected override void OnInit()
    {
        uiType = BaseUIType.UI;
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnShow()
    {
        
    }
}
