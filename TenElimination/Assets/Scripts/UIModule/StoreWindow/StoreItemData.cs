using UnityEngine;
using System.Collections;

public class StoreItemData {
    public string id;
    public string name;
    public string price;
    public string itemContent;
    public string itemImageURL;
    public StoreItemType itemType;
    public string para1;
    public string para2;
    public string effectName;

    public StoreItemData() { 
    
    }

    public StoreItemData(string id, string name, string price, string itemContent, string imageURL, StoreItemType type, string para1, string para2) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.itemContent = itemContent;
        this.itemImageURL = imageURL;
        this.itemType = type;
        this.para1 = para1;
        this.para2 = para2;
    }

}

public enum StoreItemType { 
    Block,
    Time,
    Null,
}
