using UnityEngine;
using System.Collections;

public class PlatformRotate : MonoBehaviour {

    public float speed = 2f;
    private Transform t;

    void Start () {
        t = transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.World);
    }
}
