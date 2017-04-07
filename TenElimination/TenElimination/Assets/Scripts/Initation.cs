using UnityEngine;
using System.Collections;
/// <summary>
/// 全局启动类
/// </summary>
public class Initation : MonoBehaviour {
    private ICommonState mCoreGame = new MyCoreGame();
    
    void Awake() {
        mCoreGame.Awake();
        
    }

    void Start() {
        mCoreGame.Start(); 
    }

    void Update() {
        mCoreGame.Update(Time.deltaTime);
    }

    void LateUpdate() {
        mCoreGame.LateUpdate(Time.deltaTime);
    }

    void FixedUpdate() {
        mCoreGame.FixedUpdate(Time.deltaTime);
    }

    void OnEnable() {
        mCoreGame.OnEnable();
    }

    void OnDisable() {
        mCoreGame.OnDisable();
    }

    void OnApplicationQuit() {
        AVOSCloud.AVUser.LogOut();
    }
}
