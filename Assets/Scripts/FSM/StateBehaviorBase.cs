using System;
using UnityEngine;

public abstract class StateBehaviorBase<T> : MonoBehaviour, IState where T : Enum
{
    public abstract T GetStateKey();

    public virtual void StateEnter()
    {
    }

    public virtual void StateUpdate()
    {
        
    }

    public virtual void StateExit()
    {
    }
}