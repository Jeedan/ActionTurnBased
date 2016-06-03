using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine
{

    [SerializeField]
    public Stack<IState> statesStack = new Stack<IState>();
    public Dictionary<string, IState> mStates = new Dictionary<string, IState>();

    private IState currState;

    // Use this for initialization
    public StateMachine()
    {
    }

    public IState currentState()
    {
        if (statesStack.Count > 0)
        {
            currState = statesStack.Peek();
            return currState;
        }

        return null;
    }

    public void OnUpdate()
    {
        currentState().OnUpdate();
    }

    public void ChangeState(string name)
    {
        IState newState = mStates[name];
        if (newState != null && newState != currState) // if something is wrong check currentState()
        {
            PushState(name);
        }
        else
        {
            Debug.Log(currentState().ToString() + " " + name);
            Debug.Log("dict count: " + mStates.Count);
            Debug.LogError("State does not exist, or we are already in the state");
        }
    }

    public void PushState(string name)
    {
        IState state = mStates[name];
        IState prevState = currentState();

        if (state != null && state != currentState())
        {
            if (statesStack.Count > 0)
            {
                //statesStack.Pop().OnExit();
                prevState.OnExit();
            }

            statesStack.Push(state);
            currentState().OnEnter();
        }
    }

    public void Pop()
    {
        IState popState = PopState();

        Debug.Log("dict count: " + mStates.Count);
        popState.OnExit();
        currentState().OnEnter();
    }

    public IState PopState()
    {
        if (statesStack.Count > 0)
            return statesStack.Pop();
        else
            return null;
    }

    public void AddState(string name, IState state)
    {
        if (!mStates.ContainsKey(name) && state != null)
        {
            mStates.Add(name, state);
        }
    }

    //TODO: FIX BELOWE
    // FIXME: methods without string seem to be incomplete
    public void ChangeState(IState _state)
    {
        IState prevState = currState;
        if (_state != currState && _state != null) // if something is wrong check currentState()
        {
            if (prevState != null)
                prevState.OnExit();

            currState = _state;
            currState.OnEnter();
        }
    }

    // FIXME: methods without string seem to be incomplete
    public void PushState(IState _state)
    {
        IState state = _state;
        if (state != null && state != currState) // if something is wrong check currentState()
        {
            statesStack.Push(state);
        }
    }
}