using UnityEngine;
using System.Collections;
using AVOSCloud;
/// <summary>
/// 直接继承AVObject的类有问题
/// </summary>

[AVClassName(mTableNameStr)]
public class StoreItem : AVObject {
    private const string mTableNameStr = "StoreItems";
    //private const string mItemIDStr = "objectID";
    private const string mItemImageStr = "image";
    private const string mItemContentStr = "content";
    private const string mItemPriceStr = "price";
    private const string mItemNameStr = "name";
    private const string mItemTypeStr = "type";
    private const string mPara1Str = "para1";
    private const string mPara2Str = "para2";
    private const string mEffectNameStr = "effectName";

    public StoreItem() { 
    
    }

    public static string TableName {
        get {
            return mTableNameStr;
        }
    }
    
    public string ID {
        get {
            return this.ObjectId;
        }
    }
    
    public string ImageURL {
        get {
            return ((AVFile)this[mItemImageStr]).Url.AbsoluteUri;
        }
    }
    
    public string ItemContent {
        get {
            return this[mItemContentStr].ToString();
        }
    }
    
    public string ItemPrice {
        get
        {
            return this[mItemPriceStr].ToString();
        }
    }
    
    public string ItemName {
        get {
            return this[mItemNameStr].ToString();
        }
    }

    public StoreItemType ItemType {
        get {
            string str = this[mItemTypeStr].ToString();
            switch (str) {
                case "Block": return StoreItemType.Block;
                case "Time": return StoreItemType.Time;
            }
            return StoreItemType.Null;
        }
    }

    public string Para1 {
        get {
            return this[mPara1Str].ToString();
        }
    }

    public string Para2 {
        get {
            return this[mPara2Str].ToString();
        }
    }

    public string EffectName {
        get {
            return this[mEffectNameStr].ToString();
        }
    }

}
