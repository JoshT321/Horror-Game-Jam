using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour
{
    private Camera playerCam;
    private Image cursorImage;
    public GameObject hoveredObject; 
    public LayerMask interactableLayer;
    public Ray playerAim;
    

    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main;
        cursorImage = GetComponent<Image>();
        //Cursor.lockState = CursorLockMode.Locked;
        ;
    }

    private void Update()
    {
        UpdateLookedAtObject();
    }

    public void UpdateLookedAtObject()
    {
        RaycastHit hit;
        playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)) ;

        if (Physics.Raycast(playerAim,out hit, 100f,interactableLayer ))
        {
            if (hit.transform.CompareTag("Door"))
            {
                cursorImage.color = Color.red;
                hoveredObject = hit.transform.gameObject;
                return;
            }

        }

        hoveredObject = null;
        cursorImage.color = Color.white;
        return;
    }

}
