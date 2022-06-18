using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private protected Werewolf werewolf;

    public Idle(Werewolf _werewolf)
    {
        werewolf = _werewolf; 
    }
    
    public override IEnumerator Enter()
    {
        Debug.Log("Wolf entering Idle State");
        werewolf.DebugText.text = "Idle:Enter";
        
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        werewolf.DebugText.text = "Idle:Execute";
        yield return null;
    }

    public override IEnumerator Exit()
    {
        werewolf.DebugText.text = "Idle:Exit";
        yield return null;
    }
}
