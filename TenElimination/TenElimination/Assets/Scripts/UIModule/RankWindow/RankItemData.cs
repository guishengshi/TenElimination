using UnityEngine;
using System.Collections;
/// <summary>
/// 排行榜物体信息模型类
/// </summary>

public class RankItemData {
    public string userName;
    public string userNickName;
    public int userMaxScore;
    public int rankNumber;
    
    public static RankItemData Empty {
        get {
            return new RankItemData("", "", -1);
        }
    }

    public RankItemData() { 
    
    }

    public RankItemData(string name, string nickname, int score) {
        this.userName = name;
        this.userNickName = nickname;
        this.userMaxScore = score;
    }
}
