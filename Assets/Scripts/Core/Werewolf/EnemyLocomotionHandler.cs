using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Numerics;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnemyLocomotionHandler : MonoBehaviour
{
    [Header("Pathfinding")]
    public Vector3 TargetDestination;

    public float MoveSpeed;
    public bool isMoving;
    public float MAX_SEE_AHEAD;

    private Rigidbody rb; 
    private WaitForSeconds buffer;
    private AIPath aiPath;
    private Seeker seeker;
    public Path path;
    private int currentWaypoint;
    public float nextWaypointDistance;
    private bool reachedEndOfPath = false;

    public IEnumerator MoveToLocation_Holder;

    [Header("Steering")] 
    private bool isSteering; 
    public float rayRange = 2;

    public float targetVelocity = 10f;

    [Header("Debugging")] 
    public float numberOfRays;

    public float angle = 90f; 

    private void Awake()
    {
        //We can DEFINITELY decrease this buffer for more rapid pathfinding. Setting to 0.5f as a baseline
        buffer = new WaitForSeconds(0.5f);
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>(); 
        //Init();

         //StartCoroutine(MovetoLocation_Co(new Vector3(0,0.430000007F,-3.3599999F)));
        //StartCoroutine(Steering_Co());
    }


    private void Init()
    {
        aiPath.enableRotation = false;
        
    }

    public bool GetDestinationStatus()
    {
        if (aiPath.remainingDistance <= 0.01f)
            return true;
        else return false;
    }
    

    //Set to 0 to stop movement
    public void SetMoveSpeed(float newMoveSpeed) => aiPath.maxSpeed = newMoveSpeed;


    public void MoveToLocation(Vector3 location)
    {
        if (MoveToLocation_Holder == null)
        {
            MoveToLocation_Holder = MovetoLocation_Co(location);
            StartCoroutine(MoveToLocation_Holder);
        }
        else
        {
            // StopCoroutine(MoveToLocation_Holder);
            // MoveToLocation_Holder = MovetoLocation_Co(location);
            // StartCoroutine(MoveToLocation_Holder);
            TargetDestination = location;
            UpdatePath(TargetDestination);
        }
        
    }
    public IEnumerator MovetoLocation_Co(Vector3 location)
    {
        TargetDestination = location;
        aiPath.canMove = true;
        

        seeker.StartPath(transform.position, TargetDestination, OnPathGenerationCompleted);
        while (path == null)
            yield return null;
        
        isMoving = true;
        while (!aiPath.reachedEndOfPath)
        {
            UpdatePath(TargetDestination);
            yield return buffer;
        }

        isMoving = false;

        yield return null;
    }

    public IEnumerator Steering_Co()
    {
        while (true)
        {
            var deltaPosition = Vector3.zero;
            for (int i = 0; i < numberOfRays; i++)
            {
                var rotation = this.transform.rotation;
                var rotationMod = Quaternion.AngleAxis((i / numberOfRays - 1) * angle * 2 - angle, this.transform.up);
                var direction = rotation * rotationMod * -Vector3.forward;

                RaycastHit hit;
                var ray = new Ray(this.transform.position, direction);
            
                if (Physics.Raycast(ray, out hit, rayRange))
                {
                    deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction; 
                    rb.AddForce(deltaPosition * Time.deltaTime);
                    //aiPath.Move(deltaPosition); 
                    Debug.Log("Did Hit");
                }
                else
                {
                    deltaPosition += (1.0f / numberOfRays) * targetVelocity * direction;
                }

                //this.transform.position += deltaPosition * Time.deltaTime;
            }
            yield return null;
        }

        yield return null;
    }


    
    void OnPathGenerationCompleted(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
//             ValidatePath(p);
// //            Debug.Log("Set the path --------------");
//             currentWaypoint = 0 ;
        }
        else
            Debug.Log("Error in path generation " + p.errorLog);
    }
    
    void UpdatePath(Vector3 targetDestination)
    {
        if(seeker == null)
            return;
        
        if(seeker.IsDone()) 
            seeker.StartPath(this.transform.position, targetDestination, OnPathGenerationCompleted);
    }
    
    


    private void OnDrawGizmos()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / numberOfRays - 1) * angle * 2 - angle, this.transform.up);
            var direction = rotation * rotationMod * -Vector3.forward; 
            Gizmos.DrawRay(this.transform.position, direction);
        }
    }
}
