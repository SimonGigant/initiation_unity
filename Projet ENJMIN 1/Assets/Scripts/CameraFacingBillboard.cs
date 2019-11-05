using UnityEngine;
using System.Collections;
 
 
public class CameraFacingBillboard : MonoBehaviour
{
 
    public Camera m_Camera;
	public bool amActive =true;
	public bool autoInit =true;
 
	void Awake(){
		if (autoInit == true){
			m_Camera = Camera.main;
			amActive = true;
		}
	}
 
    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate(){
        if(amActive==true){
            transform.LookAt(transform.position + m_Camera.transform.rotation * -Vector3.back, m_Camera.transform.rotation * Vector3.up);
        }
    }
}