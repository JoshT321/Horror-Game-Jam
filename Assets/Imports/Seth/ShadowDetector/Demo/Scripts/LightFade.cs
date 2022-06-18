using UnityEngine;
using System.Collections;

public class LightFade : MonoBehaviour {

    private Light l;
    public float fadeLimit = 0.1f;
    private float startIntensity = 1;
    private bool trigerLight = false;
    public float speed = 1f;

	void Start () {
        l = transform.GetComponent<Light>();
        startIntensity = l.intensity;
    }
	

	void Update () {
	    if(trigerLight)
        {
            if(l.intensity > fadeLimit)
            {
                l.intensity -= Time.deltaTime * speed;
            }
            else
            {
                trigerLight = !trigerLight;
            }
        }
        else
        {
            if (l.intensity < startIntensity)
            {
                l.intensity += Time.deltaTime * speed;
            }
            else
            {
                trigerLight = !trigerLight;
            }
        }

    }

}
