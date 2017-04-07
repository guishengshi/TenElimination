using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AVOSCloud;
using System.Threading.Tasks;
/// <summary>
/// 查询类
/// </summary>


public class Quary {
    public delegate void OnVerifyUserNameCompleteCall(bool isSuccess);
    public delegate void OnVerifyNickNameCompleteCall(bool isSuccess);
    public delegate void OnGetStoreItemsCall(bool isSuccess, List <StoreItemData> datas);
    public delegate void OnGetStoreItemCall(bool isSuccess, StoreItemData data);
    public delegate void OnGetRankItemsCall(bool isSuccess, List <RankItemData> datas);

    public static void VerifyUserName(string name, OnVerifyUserNameCompleteCall onCompleteCall) {
        AVQuery<User> query = new AVQuery<User>().WhereEqualTo(User.UserNameStr, name);
        query.CountAsync().ContinueWith((t) => {
            if (t.IsFaulted)
            {
                onCompleteCall(false);
                Debug.Log(t.Exception);
            }
            else {
                if (t.Result == 0)
                {
                    onCompleteCall(true);
                }
                else
                {
                    onCompleteCall(false);
                }
               
            }
        });
    }

    public static void VerifyNickName(string name, OnVerifyNickNameCompleteCall onCompleteCall) {
        AVQuery<User> query = new AVQuery<User>().WhereEqualTo(User.NickNameStr, name);
        query.CountAsync().ContinueWith((t) =>
        {
            if (t.IsFaulted)
            {
                onCompleteCall(false);
                Debug.Log(t.Exception);
            }
            else
            {
                if (t.Result == 0)
                {
                    onCompleteCall(true);
                }
                else
                {
                    onCompleteCall(false);
                }

            }
        });
    }

    public static void GetAllStoreItems(OnGetStoreItemsCall call) {
        AVQuery<StoreItem> quary = new AVQuery<StoreItem>();
        quary.FindAsync().ContinueWith((s) =>
        {
            if (!s.IsFaulted)
            {
                List<StoreItemData> datas = new List<StoreItemData>();
                foreach (StoreItem item in s.Result)
                {
                    datas.Add(new StoreItemData(item.ID, item.ItemName, item.ItemPrice, item.ItemContent, item.ImageURL, item.ItemType, item.Para1, item.Para2));
                }
                call(true, datas);
            }
            else {
                call(false, null);
            }
        });
    }

    public static void GetStoreItem(string itemID, OnGetStoreItemCall call) {
        AVQuery<StoreItem> quary = new AVQuery<StoreItem>();
        quary.GetAsync(itemID).ContinueWith((t) => {
            if (!t.IsFaulted)
            {
                StoreItemData data = new StoreItemData(t.Result.ID, t.Result.ItemName, t.Result.ItemPrice, t.Result.ItemContent, t.Result.ImageURL, t.Result.ItemType, t.Result.Para1, t.Result.Para2);
                data.effectName = t.Result.EffectName;
                call(true, data);
            }
            else {
                call(false, null);
            }
        });
    }
    
    public static void GetStoreItems(List<string> itemsID, OnGetStoreItemCall call) {
        AVQuery<StoreItem> quary = new AVQuery<StoreItem>().WhereContainedIn<string>("objectID", itemsID);
        quary.FindAsync().ContinueWith((t) =>
        {
            if (!t.IsFaulted)
            {
                foreach (StoreItem item in t.Result)
                {
                    StoreItemData data = new StoreItemData(item.ID, item.ItemName, item.ItemPrice, item.ItemContent, item.ImageURL, item.ItemType, item.Para1, item.Para2);
                    data.effectName = item.EffectName;
                    call(true, data);
                }
            }
            else
            {
                call(false, null);
            }
        });
    }

    public static void GetMaxScore10Users(string modeName, OnGetRankItemsCall call) {
        AVQuery<AVUser> quary = new AVQuery<AVUser>().OrderByDescending(modeName).Limit(10);
        quary.FindAsync().ContinueWith((t) => {
            if (!t.IsFaulted)
            {
                List<RankItemData> list = new List<RankItemData>();
                foreach (AVUser user in t.Result)
                {
                    list.Add(new RankItemData(user.Username, user[User.NickNameStr].ToString(), int.Parse(user[modeName].ToString())));
                }
                call(true, list);
            }
            else {
                call(false, null);
            }
        });
    }

}
