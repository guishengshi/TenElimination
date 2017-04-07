using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 状态机管理类
/// </summary>
public class StateManager {
	#region 回调接口
	//状态转换时的委托
	public delegate void BetweenSwitchState (IState prevState, IState nextState, object para1, object para2);
	//状态转换时的回调
	public BetweenSwitchState BetweenStateCall;
	#endregion
	
	#region 变量
	//存储所有状态的容器
	private Dictionary <uint, IState> stateDic;
	#endregion

	#region 属性
	//当前状态
	private IState mCurrentState = null;
	public IState CurrentState {
		get {
			return mCurrentState;
		}
	}
	public int CurrentStateID {
		get {
			return mCurrentState == null ? -1 : (int)mCurrentState.GetStateID ();
		}
	}
	#endregion

	#region 方法
	//构造方法
	public StateManager () {
		stateDic = new Dictionary<uint, IState> ();
	}
	//注册状态的方法
	public void RigisterState (IState state) {
		if (state != null && !stateDic.ContainsKey (state.GetStateID ())) {
			stateDic.Add (state.GetStateID (), state);
		}
	}
	//移除状态的方法
	public void RemoveState (uint stateID) {
		if (stateDic.ContainsKey (stateID)) {
			stateDic.Remove (stateID);
		}
	}
	//获取一个状态
	public IState GetState (uint stateID) {
		IState state = null;
		stateDic.TryGetValue (stateID, out state);
		return state;
	}
	//停止当前状态
	public void StopCurrentState (object para1, object para2) {
		if (mCurrentState == null) {
			return;
		}
        IState tempState = mCurrentState;
        mCurrentState = null;
		tempState.OnExit (null, para1, para2);
	}
	//转换状态的方法.两个参数用于状态转换时传递的参数
	public void SwitchState (uint stateID, object para1, object para2) {
		if (mCurrentState != null && mCurrentState.GetStateID () == stateID) {
			return;
		}
		if (!stateDic.ContainsKey (stateID)) {
			return;
		}
		IState oldState = mCurrentState;
		stateDic.TryGetValue (stateID, out mCurrentState);
        if (oldState != null) {
            oldState.OnExit(mCurrentState, para1, para2);
        }
		if (oldState != null && BetweenStateCall != null) {
			BetweenStateCall (oldState, mCurrentState, para1, para2);
		}
		mCurrentState.OnEnter (this, oldState, para1, para2);
	}
	//判断一个状态是否在进行
	public bool IsCurrent (uint stateID) {
		if (!stateDic.ContainsKey (stateID)) {
			return false;
		}
		return mCurrentState == null ? false : mCurrentState.GetStateID () == stateID;
	}
	public void OnUpdate (float timeDelta) {
		if (mCurrentState != null) {
            mCurrentState.OnUpdate(timeDelta);
		}
	}
    public void OnFixUpdate(float timeDelta)
    {
		if (mCurrentState != null) {
			mCurrentState.OnFixUpdate (timeDelta);
		}
	}
    public void OnLateUpdate(float timeDelta)
    {
		if (mCurrentState != null) {
			mCurrentState.OnLateUpdate (timeDelta);
		}
	}
	#endregion
}
