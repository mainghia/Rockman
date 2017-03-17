using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(CustomPhysicCharacterController))]
public class PlayerController : MonoBehaviour
{
	public float runspeed;
	public float jumpspeed;
	public float dashSpeed;
	public bool grounded, canDoubleJump, wallSliding, hovering;
	public bool facingRight = true;

	private Collider2D coll;
	private CustomPhysicCharacterController characterController;
	public Vector2 currentVelocity;

	private Animator anim;


	private void Awake ()
	{
		characterController = GetComponent<CustomPhysicCharacterController> ();
		coll = GetComponent<Collider2D> ();
		anim = GetComponent<Animator> ();

	}

	private void Start ()
	{
		currentVelocity = new Vector2 (0, 0);
	}

	private void Jump ()
	{
		currentVelocity = new Vector2(currentVelocity.x, jumpspeed * Time.fixedDeltaTime);
		grounded = false;
	}

	private void DoubleJump(){
		currentVelocity = new Vector2(currentVelocity.x, jumpspeed * Time.fixedDeltaTime);
		grounded = false;
	}

	private void handleWallSliding ()
	{
		if (wallSliding) {
			currentVelocity = new Vector2 (currentVelocity.x, -0.2f * Time.fixedDeltaTime);
		}
	}

	private void Run ()
	{
		Flip ();
		currentVelocity = new Vector2 (Input.GetAxis ("Horizontal") * runspeed * Time.fixedDeltaTime, currentVelocity.y);
		anim.SetFloat ("runspeed",Input.GetAxis ("Horizontal"));
	}

	private void Dash ()
	{
		Flip ();
		int dashDirection = facingRight ? 1 : -1;
		currentVelocity = new Vector2 (dashDirection * dashSpeed * Time.fixedDeltaTime, currentVelocity.y);
		anim.SetFloat ("runspeed",dashSpeed * Time.fixedDeltaTime);
	}

	private void Update ()
	{
		if (characterController.collisionInfo.collideBottom) {
			grounded = true;
			canDoubleJump = false;
			wallSliding = false;
		}
		Run ();

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (grounded) {
				Jump ();
				canDoubleJump = true;
			} else {
				if (canDoubleJump) {
					canDoubleJump = false;
					DoubleJump ();
				}
			}
		}
		if (!grounded && characterController.collisionInfo.collideWallLeft && Input.GetAxis ("Horizontal") < 0 ||
			!grounded && characterController.collisionInfo.collideWallRight && Input.GetAxis ("Horizontal") > 0) {
			wallSliding = true;
			handleWallSliding ();
		} else {
			wallSliding = false;
		}

		if (Input.GetKeyDown (KeyCode.C))
			Dash ();	
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
	{	if (!wallSliding) {	
			currentVelocity += Physics2D.gravity * 0.5f * Time.fixedDeltaTime;
		}
		characterController.Move (currentVelocity);
		if (characterController.collisionInfo.collideBottom) {
			currentVelocity.y = 0;		
		}

	}
}
