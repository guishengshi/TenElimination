using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 用户信息类
/// </summary>

public class PlayerDataModel {
    public string name;
    public string nickName;
    public List<string> itemsID = new List<string> ();
    public int customModeScore;
    public int transformModeScore;
    public int topSpeedModeScore;
    public int gold;

    public PlayerDataModel() { 
    
    }

    public PlayerDataModel(string name, string nickName) {
        this.name = name;
        this.nickName = nickName;
    }
}
