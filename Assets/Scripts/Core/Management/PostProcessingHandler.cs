using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingHandler : MonoBehaviour
{
    public static PostProcessingHandler Instance;
    public PostProcessVolume postProcessVolume;
    public ColorGrading colorGrading;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying GameManager");
            Destroy(this.gameObject);
        }
        else
            Instance = this;

        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    private void Start()
    {
        SetBrightness(SaveData.current.profile.brightness);
    }


    public void SetBrightness(float newValue) => colorGrading.postExposure.value = newValue;
}
