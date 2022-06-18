using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Werewolf : MonoBehaviour
{
    public StateMachine StateMachine;
    public Animator anim; 
    public EnemyLocomotionHandler LocomotionHandler;
    public EnemyDetectionHandler detectionHandler;
    public WaypointHandler WaypointHandler;
    
    public TMP_Text DebugText;
    

    //States
    public Patrol Patrol;
    public Pursuit Pursuit;

    private void Awake()
    {
        WaypointHandler = GameObject.FindObjectOfType<WaypointHandler>(); 
    }

    private void Start()
    {
        DebugText = transform.Find("DebugText").Find("Text").GetComponent<TMP_Text>();
        LocomotionHandler = GetComponent<EnemyLocomotionHandler>();
        detectionHandler = GetComponent<EnemyDetectionHandler>(); 
        
        
        StateMachine = new StateMachine(this);
        Patrol = new Patrol(this);
        Pursuit = new Pursuit(this);
        StateMachine.ChangeState(Patrol);



    }
}
