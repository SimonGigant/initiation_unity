using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
	public GameObject focus;
	public float maxDistance;
	public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position,focus.transform.position);
		if(distance > maxDistance*1.5){
			Vector3 direction = (focus.transform.position-transform.position).normalized;
			transform.position += direction*speed*4;
		}else if(distance > maxDistance){
			Vector3 direction = (focus.transform.position-transform.position).normalized;
			transform.position += direction*speed;
		}
    }
}
