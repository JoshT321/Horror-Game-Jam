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
    public LayerMask blockLayer;
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
        playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(playerAim, out hit, Mathf.Infinity, interactableLayer))
        {
            if (hit.transform.CompareTag("Blocker"))
            {
                hoveredObject = null;
                cursorImage.color = Color.white;
                return;
            }
            
            if (hit.transform.CompareTag("Pickup"))
            {
                cursorImage.color = Color.green;
                hoveredObject = hit.transform.gameObject;
                return;
            }
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
