using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/// <summary>
/// 游戏物体的对象池，为了优化修改策略，池子在游戏加载时存储所有方块
/// </summary>

public interface IResetable
{
    void Reset();
}

public class ObjectPoolWithReset<T> where T : class, IResetable, new()
{
    private Stack<T> m_objectStack;

    private Action<T> m_resetAction;
    private Func<T> mNew;
    private Action<T> m_clearAction;

    public ObjectPoolWithReset(int initialBufferSize, Func<T> NewFunc, Action<T>
        ResetAction = null, Action <T> clearAction = null)
    {
        m_objectStack = new Stack<T>(initialBufferSize);
        m_resetAction = ResetAction;
        mNew = NewFunc;
        m_clearAction = clearAction;
        LoadingInAdvance(initialBufferSize);
    }

    private void LoadingInAdvance(int size) {
        for (int i = 0; i < size; i++) {
            T t = mNew();
            m_objectStack.Push(t);
        }
    }

    public T New()
    {
        if (m_objectStack.Count > 0)
        {
            T t = m_objectStack.Pop();
            //自行重置
            t.Reset();

            if (m_resetAction != null)
                m_resetAction(t);

            return t;
        }
        else
        {
            if (mNew == null) {
                Debug.LogError("初始化方法为空");
            }
            T t = mNew ();
            return t;
        }
    }

    public void Store(T obj)
    {
        m_objectStack.Push(obj);
    }

    public void Clear()
    {
        while (m_objectStack.Count != 0) {
            T t = m_objectStack.Pop();
            if (m_clearAction != null) {
                m_clearAction(t);
            }
        }
        m_objectStack.Clear();
    }  
}
