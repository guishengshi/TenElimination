using UnityEngine;
using System.Collections;
/// <summary>
/// 用户管理模块
/// </summary>

public class PlayerManager  {
    private CoreGame mCoreGame;

    public PlayerManager(CoreGame coreGame) {
        mCoreGame = coreGame;
    }

    private PlayerDataModel mPlayerData;

    public PlayerDataModel MPlayerData
    {
        get { return mPlayerData; }
        set { mPlayerData = value; }
    }

    public CoreGame MCoreGame {
        get {
            return mCoreGame;
        }
    }

}
