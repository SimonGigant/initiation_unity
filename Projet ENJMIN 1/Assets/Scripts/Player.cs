using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed;
	public float dashSpeed;
	public float dashDuration;
    public float jumpForce;
	public float gravityScale;
	public Camera cam;
	public AnimationCurve vertigoCurve;
	
	private CharacterController controller;
	private Vector3 moveDirection;
	private bool isDashing = false;
	private float counterDash;

    public GameObject[] dashParticles;
    public int sizeDashParticles;
    public GameObject prefabDashParticle;
    public GameObject spriteObject;
    private Vector2 dir;
    private Vector3 previousCamRotation;

    private Animator animator;

    void Start ()
    {
		controller = GetComponent<CharacterController>();
		moveDirection = new Vector3(0f,0f,0f);
        dashParticles = new GameObject[sizeDashParticles];
        for (int i = 0; i < sizeDashParticles; ++i)
        {
            dashParticles[i] = Instantiate(prefabDashParticle, new Vector3(0, 0, 0), Quaternion.identity);
        }
        animator = spriteObject.GetComponent<Animator>();
        previousCamRotation = cam.transform.eulerAngles;
    }

    void FixedUpdate()
    {
		if(isDashing){
			Dash();
		}else{
			float moveFB = Input.GetAxis("Vertical");
			float moveLR = Input.GetAxis("Horizontal");
			Vector3 fwd = moveFB * Time.deltaTime*speed*cam.transform.forward;
			Vector3 side = moveLR * Time.deltaTime*speed*cam.transform.right;
			moveDirection = new Vector3(fwd.x+side.x,moveDirection.y,fwd.z+side.z);
            animator.SetBool("running", fwd.magnitude > 0.01f || side.magnitude > 0.01f);
            if (fwd.magnitude > 0.01f || side.magnitude > 0.01f)
            {
                dir = new Vector2(moveDirection.x, moveDirection.z);
                float angle = Vector2.SignedAngle(new Vector2(cam.transform.forward.x, cam.transform.forward.z), dir);
                if ((angle < 179 && angle > -179)&&(angle > 45 || angle < -45))
                {
                    spriteObject.GetComponent<SpriteRenderer>().flipX = angle < 0;
                }
                animator.SetFloat("direction", angle);
            }
            else
            {
                if (previousCamRotation != cam.transform.eulerAngles)
                {
                    float angle = Vector2.SignedAngle(new Vector2(cam.transform.forward.x, cam.transform.forward.z), dir);
                    if ((angle < 179 && angle > -179) && (angle > 45 || angle < -45))
                    {
                        spriteObject.GetComponent<SpriteRenderer>().flipX = angle < 0;
                    }
                    animator.SetFloat("direction", angle);
                }
            }
			if(controller.isGrounded && Input.GetButtonDown("Jump")){
				moveDirection.y = jumpForce;
			}else if(controller.isGrounded && Input.GetButtonDown("Fire1")){
				moveDirection = (new Vector3 ((fwd.x + side.x)* dashSpeed / speed, moveDirection.y, (fwd.z + side.z) *dashSpeed/speed));
				isDashing = true;
				counterDash = 0f;
				//Gamefeel.Instance.InitVertigo(cam, this.gameObject, dashDuration, 3f, vertigoCurve);
				Gamefeel.Instance.InitScreenshake(cam, dashDuration, 0.15f, vertigoCurve);
			}
		}			
		moveDirection.y += Physics.gravity.y*Time.deltaTime*gravityScale;
		controller.Move(moveDirection);
    }
	
	void Dash()
	{
		counterDash+=Time.deltaTime;
        GenerateDashParticle();

        if (counterDash > dashDuration){
			isDashing = false;
		}
	}

    void GenerateDashParticle()
    {
        for(int i = 0; i < sizeDashParticles; ++i)
        {
            if (dashParticles[i].GetComponent<FadeAway>().Create(this.gameObject, spriteObject))
            {
                break;
            }
        }
    }
}