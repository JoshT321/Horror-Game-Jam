using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Camera loadingCamera;
    public GameObject backgroundObj;
    public Image autosaveImage;
    private Animator autoSaveAnim;
    private TMP_Text autoSaveText;
    public IEnumerator SetScreenHider_Holder; 

    private void Awake()
    {
        backgroundObj = transform.Find("Background").gameObject;
        
        autoSaveText = transform.Find("Background").Find("Autosave Text").GetComponent<TMP_Text>();
        autosaveImage = transform.Find("Background").Find("Autosave Icon").Find("Image").GetComponent<Image>();
        autoSaveAnim = autosaveImage.GetComponentInParent<Animator>();
        autoSaveAnim.Play("Hide");
        
        
        autosaveImage.color = new Color(0f, 0f, 0f, 0f);
        autoSaveText.color = new Color(0f, 0f, 0f, 0f);
        backgroundObj.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
        
    }
    
    

    public void SetScreenHider(bool newStatus)
    {
        if (SetScreenHider_Holder == null)
        {
            SetScreenHider_Holder = SetLoadingScreen_Co(newStatus);
            StartCoroutine(SetScreenHider_Holder);
        }
        else
        {
            StopCoroutine(SetScreenHider_Holder);
            SetScreenHider_Holder = SetLoadingScreen_Co(newStatus);
            StartCoroutine(SetScreenHider_Holder);
        }
    }


    public IEnumerator SetLoadingScreen_Co(bool newStatus)
    {

        Image image = backgroundObj.GetComponent<Image>();

        float startAlpha = newStatus ? 0f : 1f;
        float targetAlpha = newStatus ? 1f : 0f; 
        float elapsedTime = 0f;
        float timeToWait = 1f;
        
         while (elapsedTime <= timeToWait)
        {
            if (newStatus)
            {
                elapsedTime = 5f;
            }
            Debug.Log($"Background current alpha: {image.color.a}   Target Alpha: {targetAlpha}");
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / timeToWait);
            image.color = new Color(0f, 0f, 0f, newAlpha);
            if (!newStatus)
            {
                autosaveImage.color = new Color(255f,255f,255f, newAlpha);
                autoSaveText.color = new Color(255f, 255f, 255f, newAlpha);
            }
            yield return null;
        }
        //image.gameObject.SetActive(false);

        elapsedTime = 0f;
        timeToWait = 3f;
        
        autoSaveAnim.Play("Autosave");
        while (elapsedTime <= timeToWait)
        {
            if (!newStatus)
            {
                
                break;
            }
            Debug.Log("Changing text and autosave alpha to " + newStatus);
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / timeToWait);
            
            autosaveImage.color = new Color(255f,255f,255f, newAlpha);
            autoSaveText.color = new Color(255f, 255f, 255f, newAlpha);
            yield return null;
        }
        
    }
}
