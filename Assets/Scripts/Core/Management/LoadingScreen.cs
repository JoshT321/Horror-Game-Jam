using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Camera loadingCamera;
    public GameObject screenHider;
    public IEnumerator SetScreenHider_Holder; 

    private void Awake()
    {
        screenHider = transform.Find("ScreenHider").gameObject; 
    }

    public void SetScreenHider(bool newStatus)
    {
        if (SetScreenHider_Holder == null)
        {
            SetScreenHider_Holder = SetScreenHider_Co(newStatus);
            StartCoroutine(SetScreenHider_Holder);
        }
        else
        {
            StopCoroutine(SetScreenHider_Holder);
            SetScreenHider_Holder = SetScreenHider_Co(newStatus);
            StartCoroutine(SetScreenHider_Holder);
        }
    }


    public IEnumerator SetScreenHider_Co(bool newStatus)
    {

        Image image = screenHider.GetComponent<Image>();

        float startAlpha = image.color.a;
        float targetAlpha = newStatus ? 255f : 0f; 
        float elapsedTime = 0f;
        float timeToWait = 0.5f;
        
        while (elapsedTime <= timeToWait)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / timeToWait);
            image.color = new Color(0f, 0f, 0f, newAlpha);
            yield return null;
        }
    }
}
