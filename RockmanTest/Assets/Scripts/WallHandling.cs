using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHandling : MonoBehaviour {
	private CustomPhysicCharacterController characterController;
	public bool wallSliding;
	// Use this for initialization
	void Start () {
		characterController = GetComponent<CustomPhysicCharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!PlayerController.Instance.grounded  && (Input.GetAxis ("Horizontal") < 0) && characterController.collisionInfo.collideLeft ||
			!PlayerController.Instance.grounded  && (Input.GetAxis ("Horizontal") > 0) && characterController.collisionInfo.collideRight)  {
			wallSliding = true;
			handleWallSliding ();
		} else {
			wallSliding = false;
		}

	}

	private void handleWallSliding ()
	{
		if (wallSliding && (Mathf.Abs(PlayerController.Instance.currentVelocity.y) < 0.05f) ) {
			PlayerController.Instance.currentVelocity = new Vector2 (PlayerController.Instance.currentVelocity.x, -0.2f * Time.fixedDeltaTime);
		}
	}
}
