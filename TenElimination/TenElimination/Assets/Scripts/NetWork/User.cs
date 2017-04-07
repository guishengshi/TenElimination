using UnityEngine;
using System.Collections;
using AVOSCloud;
using System.Threading.Tasks;
using System.Collections.Generic;

public class User : AVUser {
    

    private static string mNickNameStr = "nickname";
    private static string mUserNameStr = "username";
    private static string mPasswordStr = "password";
    //private static string mItemsIDStr = "itemsID";
    private static string mFirstItemIDStr = "firstItemID";
    private static string mSecondItemIDStr = "secondItemID";
    private static string mThirdItemIDStr = "thirdItemID";
    private static string mFourthItemIDStr = "fourthItemID";
    private static string mCustomScoreStr = "customModeScore";
    private static string mTransfromScoreStr = "transformModeScore";
    private static string mTopSpeedScoreStr = "topSpeedModeScore";
    private static string mGoldStr = "gold";
    private string mUserName;
    private string mPassword;
    private string mNickName;

    public static string NickNameStr {
        get {
            return mNickNameStr;
        }
    }

    public static string UserNameStr {
        get {
            return mUserNameStr;
        }
    }

    public static string CustomScoreStr {
        get {
            return mCustomScoreStr;
        }
    }

    public static string TransformScoreStr {
        get {
            return mTransfromScoreStr;
        }
    }

    public static string TopSpeedScoreStr {
        get {
            return mTopSpeedScoreStr;
        }
    }

    public static string GoldStr {
        get {
            return mGoldStr;
        }
    }

    //public string UserName {
    //    get {
    //        return this[mUserNameStr].ToString();
    //    }
    //    set {
    //        mUserName = value;
    //        this[mUserNameStr] = value;
    //    }
    //}


    //public string NickName {
    //    get {
    //        return this[mNickNameStr].ToString();
    //    }
    //    set {
    //        mNickName = value;
    //        this[mNickNameStr] = value;
    //    }
    //}

    //public string Password {
    //    set {
    //        mPassword = value;
    //        this[mPasswordStr] = value;
    //    }
    //}

    //public static User CurrentUser {
    //    get {
    //        return AVUser.CurrentUser as User;
    //    }
    //}
    public static User MCurrentUser {
        get {
            return AVUser.CurrentUser as User;
        }
    }

    public string MNickName {
        get {
            return this[mNickNameStr].ToString();
        }
    }

    public string MUserName {
        get {
            return this.Username;
        }
    }

    public string CustomScore
    {
        get
        {
            return this[mCustomScoreStr].ToString();
        }
    }

    public string TransformScore
    {
        get
        {
            return this[mTransfromScoreStr].ToString();
        }
    }

    public string TopSpeedScore
    {
        get
        {
            return this[mTopSpeedScoreStr].ToString();
        }
    }

    //public List<System.Object> MStoreItems
    //{
    //    get
    //    {
    //        return this[mItemsIDStr] as List<System.Object>;
    //    }
    //}

    public delegate void OnLoginSuccessCall(PlayerDataModel model);
    public delegate void OnLoginFailedCall();
    public delegate void OnRegisterSuccessCall();
    public delegate void OnRegisterFailedCall();
    public delegate void OnSaveSuccessCall();
    public delegate void OnSaveFailedCall();


    public User(string name, string nickName, string password) {
        mUserName = name;
        mPassword = password;
        mNickName = nickName;
        this[mUserNameStr] = mUserName;
        this[mPasswordStr] = mPassword;
        this[mNickNameStr] = mNickName;
    }

    public static void Login (string name, string password, OnLoginFailedCall failedCall, OnLoginSuccessCall successCall) {
        Task task = LogInAsync(name, password);
        task.ContinueWith((Task t) => {
            if (t.IsFaulted)
            {
                failedCall();
            }
            else {
                PlayerDataModel model = new PlayerDataModel();
                model.name = CurrentUser.Username;
                model.nickName = CurrentUser[mNickNameStr].ToString();
                string[] strs = new string[4] { CurrentUser[mFirstItemIDStr].ToString(), CurrentUser[mSecondItemIDStr].ToString(), CurrentUser[mThirdItemIDStr].ToString(), CurrentUser[mFourthItemIDStr].ToString() };
                for (int i = 0; i < strs.Length; i++) {
                    if (strs[i] != "-1") {
                        model.itemsID.Add(strs[i]);
                    }
                }
                model.customModeScore = int.Parse(CurrentUser[mCustomScoreStr].ToString());
                model.transformModeScore = int.Parse(CurrentUser[mTransfromScoreStr].ToString());
                model.topSpeedModeScore = int.Parse(CurrentUser[mTopSpeedScoreStr].ToString());
                model.gold = int.Parse(CurrentUser[mGoldStr].ToString());
                successCall(model);
            }
        });
    }

    public void Register(OnRegisterFailedCall failedCall, OnRegisterSuccessCall successCall) {
        this.SignUpAsync().ContinueWith((Task t) => {
            if (t.IsFaulted)
            {
                failedCall();
            }
            else {
                successCall();
            }
        });
    }

    public static void SaveCurrentUserData (PlayerDataModel data, OnSaveSuccessCall onSuccessCall = null, OnSaveFailedCall onFailedCall = null) {
        string[] strs = new string[4] { mFirstItemIDStr, mSecondItemIDStr, mThirdItemIDStr, mFourthItemIDStr };
        for (int i = 0; i < strs.Length; i++) {
            if (i >= data.itemsID.Count)
            {
                CurrentUser[strs[i]] = "-1";
            }
            else {
                CurrentUser[strs[i]] = data.itemsID[i];
            }
        }
        CurrentUser[mCustomScoreStr] = data.customModeScore;
        CurrentUser[mTransfromScoreStr] = data.transformModeScore;
        CurrentUser[mTopSpeedScoreStr] = data.topSpeedModeScore;
        CurrentUser[mGoldStr] = data.gold;
        CurrentUser.SaveAsync().ContinueWith((t) => {
            if (!t.IsFaulted)
            {
                onSuccessCall();
            }
            else {
                onFailedCall();
            }
        });
    }

    public static void AddStoreItemID(string id, OnSaveSuccessCall onSuccessCall, OnSaveFailedCall onFailedCall) {
        string[] strs = new string[4] { mFirstItemIDStr, mSecondItemIDStr, mThirdItemIDStr, mFourthItemIDStr };
        for (int i = 0; i < strs.Length; i++)
        {
            if (CurrentUser[strs[i]].ToString().Equals("-1")) {
                CurrentUser[strs[i]] = id;
                break;
            }
        }
        CurrentUser.SaveAsync().ContinueWith((t) => {
            if (!t.IsFaulted)
            {
                onSuccessCall();
            }
            else {
                onFailedCall();
            }
        });
    }

}
