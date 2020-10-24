using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StateMachine
{
    private IState currentState;

    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>(); //list for transitions to states that need to be able to interupt any current state

    private static List<Transition> EmptyTransitions = new List<Transition>(0); //empty for memory 

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            SetState(transition.ToState);
            //Debug.Log(transition.ToState);
        }

        currentState.Tick();
    }

    public void SetState(IState state)
    {
        if(state == currentState) { return; }
        //Debug.Log("Set State to:" + state);

        currentState?.OnExit();
        currentState = state;

        transitions.TryGetValue(currentState.GetType(), out currentTransitions);
        if(currentTransitions == null)
        {
            currentTransitions = EmptyTransitions;
        }

        currentState.OnEnter();
    }

    public void AddTransition(IState fromState, IState toState, Func<bool> predicate)
    {
        if(transitions.TryGetValue(fromState.GetType(), out var tTransitions) == false)
        {
            tTransitions = new List<Transition>();
            transitions[fromState.GetType()] = tTransitions;
        }
        tTransitions.Add(new Transition(toState, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    private Transition GetTransition()
    {
        foreach (var transition in anyTransitions) //transition comming from any state. priority, transistions that HAVE to happen if conditions are met
            if (transition.Condition()) //goes over conditions in order, possible to make a priority system if nessessary
                return transition;

        foreach (var transition in currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }

    private class Transition
    {
        public Func<bool> Condition { get; }

        public IState ToState;

        public Transition(IState toState, Func<bool> condition)
        {
            ToState = toState;
            Condition = condition;
        }
    }

}
