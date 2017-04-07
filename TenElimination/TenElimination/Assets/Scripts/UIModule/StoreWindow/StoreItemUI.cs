using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreItemUI : BaseComponent {
    public RawImage itemImage;
    public Text itemContent;
    public Text itemPrice;
    public Button buyButton;

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnHide()
    {

    }

    protected override void OnShow()
    {

    }

    protected override void RenderPerFrame(float perTime)
    {
        base.RenderPerFrame(perTime);

    }

    public void UpdateUI(string tex, string content, string price) {
        StartCoroutine(LoadImage(tex));
        itemContent.text = content;
        itemPrice.text = price;
    }


    IEnumerator LoadImage(string url)
    {
        WWW w = new WWW(url);
        while (!w.isDone)
        {
            yield return null;
        }
        itemImage.texture = w.texture;
    }
}
