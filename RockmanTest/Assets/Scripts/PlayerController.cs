using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(CustomPhysicCharacterController))]
public class PlayerController : MonoBehaviour
{
	public float runspeed;
	public float dashSpeed;
	public float dashCooldown;
	public bool grounded,isDashing;
	public bool facingRight = true;

	private CustomPhysicCharacterController characterController;
	public Vector2 currentVelocity;

	public Animator anim;
	private static PlayerController _instance;

	public static PlayerController Instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<PlayerController>();
			return _instance;
		}
	}

	private void Awake ()
	{
		characterController = GetComponent<CustomPhysicCharacterController> ();
		anim = GetComponent<Animator> ();

	}

	private void Start ()
	{
		currentVelocity = new Vector2 (0, 0);
	}



	private void Run ()
	{
		Flip ();
		currentVelocity = new Vector2 (Input.GetAxis ("Horizontal") * runspeed * Time.fixedDeltaTime, currentVelocity.y);
		anim.SetFloat ("runspeed",Mathf.Abs(Input.GetAxis ("Horizontal")));
	}

	private IEnumerator Dash (float dashTime)
	{
		Flip ();
		int dashDirection = facingRight ? 1 : -1;
		float time = 0;
		isDashing = true;
		while(dashTime > time) 
		{
			time += Time.deltaTime; 
			currentVelocity = new Vector2( dashDirection*dashSpeed*Time.deltaTime,currentVelocity.y); 
			yield return 0; 
		}
		yield return new WaitForSeconds(dashCooldown); 
		isDashing = false;
		anim.SetFloat ("runspeed",dashSpeed * Time.fixedDeltaTime);
	}

	private void Update ()
	{
		if (characterController.collisionInfo.collideBottom) {
			grounded = true;
			anim.SetFloat ("jumpSpeed", currentVelocity.y);
		} else {
			grounded = false;
		}
		if (!isDashing) {
			Run ();
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			if (!isDashing) {
				StartCoroutine(Dash (0.4f));
			}
		}
				

	}

	private void Flip(){
		if (facingRight && Input.GetAxis ("Horizontal")<0) {
			transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			facingRight = false;
		} else if(Input.GetAxis ("Horizontal")>0 && !facingRight) {
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
			facingRight = true;
		}
	}

	private void FixedUpdate ()
	{	
		currentVelocity += Physics2D.gravity * 0.2f * Time.fixedDeltaTime;		
		characterController.Move (currentVelocity);
		if (characterController.collisionInfo.collideBottom) {
			currentVelocity.y = 0;		
		}

	}
}
