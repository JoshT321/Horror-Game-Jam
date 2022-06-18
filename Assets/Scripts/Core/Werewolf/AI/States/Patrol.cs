using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State
{
    private protected Werewolf werewolf;
    private int currentWaypointIndex; 
    private bool isPatrolling;
    

    public Patrol(Werewolf _werewolf)
    {
        werewolf = _werewolf; 
    }
    
    public override IEnumerator Enter()
    {
        Debug.Log("Wolf entering Patrol State. ");
        isPatrolling = true;
        werewolf.DebugText.text = "Patrol:Enter";
        werewolf.detectionHandler.Angle = 125f; 
        currentWaypointIndex = werewolf.WaypointHandler.GetIndexAtPosition(werewolf.detectionHandler.GetNearestWaypoint());
        
        
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        werewolf.DebugText.text = "Patrol:Execute";
        
        while (isPatrolling)
        {
            
            Debug.Log("Current Index " + currentWaypointIndex);
            
            werewolf.LocomotionHandler.MoveToLocation(werewolf.WaypointHandler.GetPositionAtIndex(currentWaypointIndex));
            
            yield return new WaitUntil(() => Vector3.Distance(werewolf.transform.position, werewolf.WaypointHandler.GetPositionAtIndex(currentWaypointIndex)) <= 0.4f);
            UpdateWaypointIndex();
            
            Debug.Log("Destination reached - increasing waypoint index and looping");
            

            //werewolf.LocomotionHandler.MoveToLocation(werewolf.WaypointHandler.GetPositionAtIndex(currentWaypointIndex));
            yield return null;
        }
        yield return null;
    }



    public override IEnumerator Exit()
    {
        werewolf.DebugText.text = "Patrol:Exit";
        yield return null;
    }

    public void UpdateWaypointIndex()
    {
        Debug.Log($"Current Waypoints: {currentWaypointIndex}  Maximum destinations: {werewolf.WaypointHandler.WaypointDestinations.Count}");
        currentWaypointIndex += 1;
        if (currentWaypointIndex >= werewolf.WaypointHandler.WaypointDestinations.Count)
        {
            Debug.Log("Current waypoint maxed out - restarting");
            currentWaypointIndex = 0;
        }
        
            
    }
}
