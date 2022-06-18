using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    public readonly Werewolf Werewolf;
    public StateMachine(Werewolf _Werewolf)
    {
        Werewolf = _Werewolf;
    }
    
    public void ChangeState(State newState)
    {
        if(currentState != null)
        {
                
            //Debug.Log("Stopping all coroutines on " + unit.name);
                
                
            Werewolf.StartCoroutine(currentState.Exit());
            currentState = null;
        }
        
        
        currentState = newState;
        Werewolf.StartCoroutine(currentState.Enter());
    }

    public void StopStateMachine()
    {
        //Werewolf.StartCoroutine(currentState.Exit());
        if(currentState != null)
        {
            Werewolf.StartCoroutine(currentState.Exit());
            
        }
            
        currentState = null;
    }
    
}