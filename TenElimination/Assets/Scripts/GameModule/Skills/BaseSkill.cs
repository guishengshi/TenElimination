using UnityEngine;
using System.Collections;
/// <summary>
/// 技能基类
/// </summary>

public abstract class BaseSkill {
    protected StoreItemData mStoreItemData;
    protected GameObject mEffect;
    protected GameCenter mGameCenter;

    public BaseSkill(StoreItemData data, GameCenter gameCenter) {
        mStoreItemData = data;
        mGameCenter = gameCenter;
        InitEffect();
    }

    public StoreItemData MStoreItemData {
        get {
            return mStoreItemData;
        }
    }

    protected abstract void InitEffect();

    protected abstract void DoSkill();

    protected abstract void DoEffect();

    protected virtual void OnUpdate(float timeDelta) { 
    
    }

    public void Play()
    {
        mGameCenter.MCoreGame.MPlayerManager.MPlayerData.itemsID.Remove(mStoreItemData.id);
        DoSkill();
        DoEffect();
    }

    public void Update(float timeDelta) {
        OnUpdate(timeDelta);
    }
}
