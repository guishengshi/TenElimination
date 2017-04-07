using UnityEngine;
using System.Collections;

public class TextExapleController : BaseUIController {
    public TextExapleController(GameObject parent, BaseWindow parentWindow) : base (parent, parentWindow) {

    }
    protected TextExapleModel model = new TextExapleModel ();
    protected override void Init()
    {
        GameObject g = Helper.GetPrefabTypeOfGameObject("TextExaple") as GameObject;
        myBaseUI = g.GetComponent<BaseComponent>();
        model.ID = "1";
        model.Content = "success";
        
    }
}
