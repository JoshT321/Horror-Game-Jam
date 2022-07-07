using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour
{
    public Camera playerCam;
    private Image cursorImage;
    public GameObject hoveredObject; 
    public LayerMask interactableLayer;
    public LayerMask blockLayer;
    public Ray playerAim;
    

    // Start is called before the first frame update

    private void Awake()
    {
        playerCam = Camera.main;
        cursorImage = GetComponent<Image>();
<<<<<<< Updated upstream
    }

    void Start()
    {

        //Cursor.lockState = CursorLockMode.Locked;
=======
        Cursor.lockState = CursorLockMode.Locked;
>>>>>>> Stashed changes
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

            if (hit.transform.CompareTag("Dialogue"))
            {
                cursorImage.color = Color.magenta;
                hoveredObject = hit.transform.gameObject;
                return;
            }
            


        }

        hoveredObject = null;
            cursorImage.color = Color.white;
            return;
        }
    

}
