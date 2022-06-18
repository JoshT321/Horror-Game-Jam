using UnityEngine;
using System.Collections;

public class PlatformMove : MonoBehaviour {

    public float maxTimeDirection = 3f;
    private float currTimeDirection = 0f;
    public float direction = 1f;
    public float speed = 2f;
    private Transform t;

	void Start () {
        t = transform;
	}
	
	void Update () {
        if (currTimeDirection < maxTimeDirection)
        {
            Vector3 newPos = t.position;
            newPos.z += speed * direction * Time.deltaTime;
            t.position = newPos;
            currTimeDirection += Time.deltaTime;
        } else
        {
            currTimeDirection = 0f;
            direction *= -1f;
        }
	}

}
