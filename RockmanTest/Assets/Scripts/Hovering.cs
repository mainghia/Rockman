using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hovering : MonoBehaviour {

	public bool canHover;
	public float jumpSpeed,hoverTime, hoverSpeed, hoverCooldown;
	private CustomPhysicCharacterController characterController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (PlayerController.Instance.grounded) {
				Jump ();
				PlayerController.Instance.grounded = false;
				canHover = true;
			} else {
				if (canHover && Input.GetKey(KeyCode.Space)) {
					StartCoroutine (Hover (hoverTime));
					PlayerController.Instance.grounded = false;
					canHover = false;
				}
			}				
		}

	}

	private void Jump ()
	{
		PlayerController.Instance.currentVelocity = new Vector2(PlayerController.Instance.currentVelocity.x, jumpSpeed * Time.fixedDeltaTime);
		PlayerController.Instance.grounded = false;
		PlayerController.Instance.anim.SetFloat ("jumpSpeed",jumpSpeed * Time.fixedDeltaTime);
	}

	private IEnumerator Hover (float hoverTime)
	{
		Flip ();
		int hoverDirection = PlayerController.Instance.facingRight ? 1 : -1;
		float time = 0;
		PlayerController.Instance.isGravity = false;
		PlayerController.Instance.isHovering = true;
		while(hoverTime > time) 
		{
			time += Time.deltaTime; 
			PlayerController.Instance.currentVelocity = new Vector2( hoverDirection*hoverSpeed*Time.deltaTime,0); 
			yield return 0; 
		}
		yield return new WaitForSeconds(hoverCooldown); 
		PlayerController.Instance.isHovering = false;
		PlayerController.Instance.isGravity = true;
		PlayerController.Instance.anim.SetFloat ("runspeed",hoverSpeed * Time.fixedDeltaTime);
	}

	private void Flip(){
		if (PlayerController.Instance.facingRight && Input.GetAxis ("Horizontal")<0) {
			transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			PlayerController.Instance.facingRight = false;
		} else if(Input.GetAxis ("Horizontal")>0 && !PlayerController.Instance.facingRight) {
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
			PlayerController.Instance.facingRight = true;
		}
	}
}
