using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerVision : MonoBehaviour
{
    public PlayerBase player;
    public bool isPanicking;
    public PostProcessingHandler postProcessingHandler;

    private void Awake()
    {
        player = GetComponent<PlayerBase>(); 
        postProcessingHandler = PostProcessingHandler.Instance;
    }

    private void Start()
    {
        StartPanic();
    }

    public void StartPanic()
    {
        isPanicking = true;
        StartCoroutine(Vignette_Co());
        StartCoroutine(LensDistortion_Co());
    }

    public IEnumerator Vignette_Co()
    {

        float elapsedtime = 0f;
        float timeToWait = 2f;

        while (isPanicking)
        {
            elapsedtime = 0f;
            while (elapsedtime <= timeToWait)
            {
                PostProcessingHandler.Instance.vignette.intensity.value = Mathf.Lerp(0.25f, 0.5f, elapsedtime / timeToWait);
                elapsedtime += Time.deltaTime;

                yield return null;
            }

            elapsedtime = 0;
            yield return new WaitForSeconds(0.5f);
            while (elapsedtime <= timeToWait)
            {
                PostProcessingHandler.Instance.vignette.intensity.value = Mathf.Lerp(0.5f, 0.25f, elapsedtime / timeToWait);
                
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }

    }

    public IEnumerator LensDistortion_Co()
    {
        float elapsedtime = 0f;
        float timeToWait = 1f;

        while (isPanicking)
        {
            elapsedtime = 0f;
            while (elapsedtime <= timeToWait)
            {
                PostProcessingHandler.Instance.lensDistortion.intensity.value = Mathf.Lerp(0, 0.4f, elapsedtime / timeToWait); 
                elapsedtime += Time.deltaTime;

                yield return null;
            }

            elapsedtime = 0;
            yield return new WaitForSeconds(0.5f);
            while (elapsedtime <= timeToWait)
            {
                
                PostProcessingHandler.Instance.lensDistortion.intensity.value = Mathf.Lerp(0.4f, 0, elapsedtime / timeToWait); 
                elapsedtime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
