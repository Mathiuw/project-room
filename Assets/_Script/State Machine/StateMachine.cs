using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState _currentState;

    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    // Must be called on the AI MonoBehaviour Update()
    public void Tick()
    {
        Transition transition = GetTransition();

        if (transition != null)
        {
            SetState(transition.To);
            Debug.Log("State machine transitioned to: " + transition.To);
        }

        _currentState?.Tick();
        Debug.Log("State machine state: " + _currentState.GetType());
    }

    public void SetState(IState state)
    {
        if (state == _currentState) return;

        _currentState?.OnExit();
        _currentState = state;

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);

        if (_currentTransitions == null)
        {
            _currentTransitions = EmptyTransitions;
        }

        _currentState.OnEnter();
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (!_transitions.TryGetValue(from.GetType(), out var transitionsList))
        {
            transitionsList = new List<Transition>();
            _transitions[from.GetType()] = transitionsList;
        }

        transitionsList.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _anyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        foreach (Transition transition in _anyTransitions)
        {
            if (transition.Condition())
            {
                //Debug.Log("Condition to any transition met");
                return transition;
            }
        }

        foreach (Transition transition in _currentTransitions)
        {
            if (transition.Condition())
            {
                //Debug.Log("Condition to transition met");
                return transition;
                
            }
        }
        return null;
    }
}