using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : State
{
    private Werewolf werewolf;
    private PlayerBase player;
    private bool inPursuit;
    

    public Pursuit(Werewolf _werewolf)
    {
        werewolf = _werewolf;
        player = werewolf.detectionHandler.Player; 
    }
    public override IEnumerator Enter()
    {
        Debug.Log("Wolf entering Pursuit State");
        inPursuit = true;
        werewolf.detectionHandler.Angle = 360;
        werewolf.DebugText.text = "Pursuit:Enter";
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Wolf Executing Pursuit State");
        werewolf.DebugText.text = "Pursuit:Executing";
        while (inPursuit)
        {
            werewolf.LocomotionHandler.MoveToLocation(werewolf.detectionHandler.canSeePlayer ? player.transform.position : werewolf.detectionHandler.lastKnownPosition);
            // if (werewolf.transform.position)
            // {
            //     
            // }
            yield return null;
        }
        
        yield return null;
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }
}
