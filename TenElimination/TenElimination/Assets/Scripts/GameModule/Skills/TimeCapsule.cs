using UnityEngine;
using System.Collections;

public class TimeCapsule : BaseSkill {
    private float mCutDownSpeedRate;
    private float mContinueTimer;
    private GameObject mParent;

    public TimeCapsule(StoreItemData data, GameCenter center, GameObject parent) : base(data, center) {
        mCutDownSpeedRate = float.Parse (data.para1);
        mContinueTimer = float.Parse(data.para2);
        mParent = parent;
        Helper.SetParent(mEffect.transform, mParent.transform);
        mEffect.transform.localPosition = new Vector3(0, 1.5f, -3);
    }

    protected override void InitEffect()
    {
        mEffect = Helper.GetEffect(mStoreItemData.effectName);
        
    }

    protected override void DoSkill()
    {
        mIsStartTime = true;
        mPreBlockSpeed = mGameCenter.CurrentBlockSpeed;
        mGameCenter.CurrentBlockSpeed *= mCutDownSpeedRate;
    }

    protected override void DoEffect()
    {
        mEffect.SetActive(true);
    }

    private bool mIsStartTime = false;
    private float mPreBlockSpeed = 0f;
    private float mCounter = 0f;

    protected override void OnUpdate(float timeDelta)
    {
        base.OnUpdate(timeDelta);
        if (mIsStartTime) {
            if (mCounter <= mContinueTimer)
            {
                mCounter += timeDelta;
            }
            else {
                mGameCenter.CurrentBlockSpeed = mPreBlockSpeed;
                GameObject.Destroy(mEffect.gameObject);
                mIsStartTime = false;
            }
        }
    }
}
