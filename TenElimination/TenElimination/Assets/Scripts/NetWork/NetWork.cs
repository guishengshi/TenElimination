using UnityEngine;
using System.Collections;
/// <summary>
/// 服务器框架注册类，用于向LeanCloud注册子类
/// </summary>

public class NetWork : ICommonState {
    //private CoreGame mCoreGame;

    public NetWork (CoreGame coreGame) {
        //mCoreGame = coreGame;
    }

    public void Awake()
    {
        Register<StoreItem>();
    }

    public void Start()
    {
        
    }

    public void Update(float timeDelta)
    {
        
    }

    public void LateUpdate(float timeDelta)
    {
        
    }

    public void FixedUpdate(float timeDelta)
    {
        
    }

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        
    }

    private void Register<T>() where T : AVOSCloud.AVObject, new()
    {
        AVOSCloud.AVObject.RegisterSubclass<T>();
    } 
}
