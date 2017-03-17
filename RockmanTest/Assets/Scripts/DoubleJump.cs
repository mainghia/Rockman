using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour {

	public float jumpspeed;
	public bool canDoubleJump;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (PlayerController.Instance.grounded) {
				Jump ();
				PlayerController.Instance.grounded = false;
				canDoubleJump = true;
			} else {
				if (canDoubleJump) {
					canDoubleJump = false;
					doubleJump ();
				}
			}
		}
	}

	private void Jump ()
	{
		PlayerController.Instance.currentVelocity = new Vector2(PlayerController.Instance.currentVelocity.x, jumpspeed * Time.fixedDeltaTime);
		PlayerController.Instance.grounded = false;
		PlayerController.Instance.anim.SetFloat ("jumpSpeed",jumpspeed * Time.fixedDeltaTime);
	}
	private void doubleJump(){
		PlayerController.Instance.currentVelocity = new Vector2(PlayerController.Instance.currentVelocity.x, jumpspeed * Time.fixedDeltaTime);
		PlayerController.Instance.grounded = false;
		PlayerController.Instance.anim.SetFloat ("jumpSpeed",jumpspeed * Time.fixedDeltaTime);
	}
}
