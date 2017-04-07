using UnityEngine;
using System.Collections;
/// <summary>
/// 状态接口，用于规范单一状态的行为
/// </summary>
public interface IState {
	//获取状态ID
	uint GetStateID ();
	//进入该状态
	void OnEnter (StateManager manager, IState prevState, object para1, object para2);
	//离开该状态
	void OnExit (IState nextIState, object para1, object para2);
	void OnUpdate (float timeDelta);
    void OnFixUpdate(float timeDelta);
    void OnLateUpdate(float timeDelta);
}
