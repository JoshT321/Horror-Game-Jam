using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyDetectionHandler : MonoBehaviour
{

    public bool isDetecting = true;
    public bool playerDetected;
    public float PlayerDetectionRadius;
    public float WaypointDetectionRadius; 
    public LayerMask PlayerLayer;
    public LayerMask WaypointLayer;
    public PlayerBase Player;
    public Vector3 lastKnownPosition;
    public GameObject lastKnownPositionObj;

    private Werewolf werewolf;
    [Header("Field of View")] 
    public float FOVRadius;
    public float Angle;
    public LayerMask ObstructionMask;
    public bool canSeePlayer; 

    [Header("Debugging")] 
    public bool showPlayerDetectionRadius;

    public bool showPlayerLineOfSight;

    public List<GameObject> Waypoints; 
    private void Awake()
    {
        werewolf = GetComponent<Werewolf>();
        Player = FindObjectOfType<PlayerBase>(); 
    }

    private void Start()
    {
        StartCoroutine(DetectPlayer());
    }

    private void Update()
    {
        lastKnownPositionObj.transform.position = lastKnownPosition;
    }

    public IEnumerator DetectPlayer()
    {
        WaitForSeconds buffer = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return buffer;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, FOVRadius, PlayerLayer);
        
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
                    {
                        canSeePlayer = true;
                        werewolf.StateMachine.ChangeState(werewolf.Pursuit);
                        lastKnownPosition = Player.transform.position;
                    }
                    else
                    {
                        if (canSeePlayer) lastKnownPosition = Player.transform.position;
                        canSeePlayer = false;
                    }
                }
                else
                {
                    if (canSeePlayer) lastKnownPosition = Player.transform.position;
                    canSeePlayer = false;
                }
                
            }
            else if (canSeePlayer)
            {
                lastKnownPosition = Player.transform.position;
                canSeePlayer = false;
            }
        
    }


    public Vector3 GetNearestWaypoint()
    {

        Transform closestWaypoint;
        Collider[] nearbyWaypointsArray= Physics.OverlapSphere(transform.position, WaypointDetectionRadius, WaypointLayer, QueryTriggerInteraction.Collide);
        
        if (nearbyWaypointsArray.Length == 0)
        {
            Debug.Log("no nearby waypoints detected, will walk to the 1st point");
            return Vector3.zero;
        }
        else if (nearbyWaypointsArray.Length >= 1)
        {
            closestWaypoint = nearbyWaypointsArray[0].transform;



            for (int i = 0; i < nearbyWaypointsArray.Length; i++)
            {
                Waypoints.Add(nearbyWaypointsArray[i].gameObject);
                float distanceFromClosestWaypoint =
                    Vector3.Distance(transform.position, closestWaypoint.transform.position);
                float distanceFromOtherWaypoint =
                    Vector3.Distance(transform.position, nearbyWaypointsArray[i].transform.position);

                if (distanceFromOtherWaypoint <= distanceFromClosestWaypoint)
                {
                    closestWaypoint = nearbyWaypointsArray[i].transform;
                }


            }

            Debug.Log("Closest waypoint is " + closestWaypoint.gameObject.name);
            return closestWaypoint.position;
        }

        return Vector3.zero;
    }



    private void OnDrawGizmos()
    {
        if (showPlayerDetectionRadius)
        {
            Gizmos.DrawSphere(transform.position, PlayerDetectionRadius);
            Gizmos.color = Color.red;
        }

        if (showPlayerLineOfSight)
        {
            Gizmos.DrawRay(transform.position, Player.transform.position - transform.position );
            Gizmos.color = Color.blue;
        }
        
    }
}
