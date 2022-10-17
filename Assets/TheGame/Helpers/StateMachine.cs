using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : State
{
    public T State { get; private set; }

    public void Init(T state)
    {
        State = state;
        state.Enter();
    }

    public void ChangeState(T state)
    {
        State.Exit();
        State = state;
        state.Enter();
    }
}

public abstract class State
{
    protected string debugMassage = "";
    public virtual void Enter()
    {
        debugMassage = "Entered to " + this.ToString() + " state!";
    }

    public virtual void Exit()
    {

    }
}
