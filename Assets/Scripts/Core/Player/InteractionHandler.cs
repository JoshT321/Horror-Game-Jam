using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    private IEnumerator MoveDoor_Holder;
    private PlayerBase Player;
    [Header("Doors")]
    public bool isHoldingDoor;

    public float doorForce;
    
    void Start()
    {
        Player = GetComponent<PlayerBase>();

    }

    private void Update()
    {
        if (UIManager.Instance.PlayerCursor.hoveredObject == null)
            return;
        

        if (UIManager.Instance.PlayerCursor.hoveredObject.CompareTag("Pickup") && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Picked up Item");
        }
        if (UIManager.Instance.PlayerCursor.hoveredObject.CompareTag("Door") && Input.GetKey(KeyCode.Mouse0))
            MoveDoor();
    }

    private void MoveDoor()
    {
        if (MoveDoor_Holder == null)
        {
            MoveDoor_Holder = HoldDoor_Co();
            StartCoroutine(MoveDoor_Holder);
        }
        else
        {
            if (isHoldingDoor)
                return;
            StopCoroutine(MoveDoor_Holder);
            MoveDoor_Holder = HoldDoor_Co();
            StartCoroutine(MoveDoor_Holder);
            
        }
            
    }

    private IEnumerator HoldDoor_Co()
    {
        float mouseY;
        float mouseX;
        Vector3 direction;
        //Vector3 camStartingRotation = Camera.main.transform.eulerAngles;
        isHoldingDoor = true;
        Player.playerController.canMouseLook = false;
        GameObject doorObj = UIManager.Instance.PlayerCursor.hoveredObject;
        Rigidbody doorRb = doorObj.GetComponent<Rigidbody>();
        
        while (isHoldingDoor)
        {
            if (Input.GetMouseButtonUp(0)) isHoldingDoor = false;

            //Camera.main.transform.eulerAngles = camStartingRotation;
            direction = (Player.transform.position - doorObj.transform.position) * -Input.GetAxis("Mouse Y");
           // Debug.Log(direction);
            doorRb.AddForceAtPosition(direction, doorObj.transform.position); 
            
            yield return null;
            

        }

        Player.playerController.canMouseLook = true;

    }

    public void Pickup()
    {
        //Player.InventoryHandler.AddToInventory(gameObject.GetComponent(<Pickup>()));
    }
}
