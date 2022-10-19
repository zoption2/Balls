using System.Collections.Generic;
using System;

public interface IStateMachine<TState> where TState : Enum
{
    TState State { get; }
    TState PreviousState { get; }
    void InitStates(params IState<TState>[] states);
    void ChangeState(TState state, bool exitFromPrevious = true);
}

public abstract class BaseStateMachine<T> : IStateMachine<T> where T : Enum
{
    public T State { get; protected set; }
    public T PreviousState { get; protected set; }

    private readonly Dictionary<T, IState<T>> _statesData = new();

    public void ChangeState(T state, bool exitFromPrevious = true)
    {
        var currentState = _statesData[State];
        if (exitFromPrevious)
        {
            currentState?.Exit();
        }
        PreviousState = State;
        State = state;
        currentState = _statesData[state];
        currentState.Enter();
    }

    public void InitStates(params IState<T>[] states)
    {
        for (int i = 0, j = states.Length; i < j; i++)
        {
            _statesData.Add(states[i].State, states[i]);
        }
    }
}

public interface IState<T> where T : Enum
{
    T State { get; }
    void Enter();
    void Exit();
}

public abstract class BaseState<T> : IState<T> where T : Enum
{
    public abstract T State { get; }
    protected string debugMassage = "";
    public virtual void Enter()
    {
        debugMassage = string.Format("Entered to {0} state!", nameof(T));
    }

    public virtual void Exit()
    {

    }
}
