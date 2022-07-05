using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.Converters;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using LensDistortion = UnityEngine.Rendering.Universal.LensDistortion;
using Vignette = UnityEngine.Rendering.PostProcessing.Vignette;

public class PostProcessingHandler : MonoBehaviour
{
    public static PostProcessingHandler Instance;
    public Volume volume;
    public VolumeProfile volumeProfile;
    public ColorAdjustments colorAdjustments;
    public UnityEngine.Rendering.Universal.Vignette vignette;
    public LensDistortion lensDistortion;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying GameManager");
            Destroy(this.gameObject);
        }
        else
            Instance = this;

        volume = GetComponent<Volume>();
        
        volume.profile.TryGet(typeof(ColorAdjustments), out colorAdjustments);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lensDistortion);
        
        vignette.active = true;
        



    }

    private void Start()
    {
        SetBrightness(SaveData.current.profile.brightness);
        
    }


    public void SetBrightness(float newValue) => colorAdjustments.postExposure.value = newValue;
}
