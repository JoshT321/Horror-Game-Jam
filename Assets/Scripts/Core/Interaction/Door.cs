using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private HingeJoint hinge;
    private Rigidbody rb;
    public float weight;
    public bool isClosed;


    private void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (hinge.angle <= 1f && hinge.angle >= -1f && rb.velocity.magnitude > 0.1f && rb.velocity.magnitude <= 0.8f)
        {
            isClosed = true;
            Debug.Log("Door Closed");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

        }
    }
}
