using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class CustomPhysicCharacterController : MonoBehaviour {

	public int numberOfRayHorizontal;
	public int numberOfRayVertical;

	public LayerMask collisionMask,wallCollisionMask;
	private BoxCollider2D col;
	private Bounds colliderBounds;
	private float skinWidth = 0.1f;
	private RaycastOrigins raycastOrigins;
	private float raySpacingHorizontal;
	private float raySpacingVertical;
	public CollisionInfo collisionInfo; 

	private void Awake(){
		col = GetComponent<BoxCollider2D> ();
		CalculateBounds ();
		CalculateRaySpacing();
	}

	public void CalculateBounds(){
		colliderBounds = col.bounds;
		colliderBounds.Expand (-skinWidth*2);
		raycastOrigins = new RaycastOrigins ();
		raycastOrigins.topLeft = new Vector2 (colliderBounds.min.x,colliderBounds.max.y);
		raycastOrigins.topRight = new Vector2 (colliderBounds.max.x,colliderBounds.max.y);
		raycastOrigins.bottomLeft = new Vector2 (colliderBounds.min.x,colliderBounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (colliderBounds.max.x,colliderBounds.min.y);
	}

	private void CalculateRaySpacing(){
		numberOfRayHorizontal = numberOfRayHorizontal < 2 ? 2 : numberOfRayHorizontal;
		numberOfRayVertical = numberOfRayVertical < 2 ? 2 : numberOfRayVertical;

		raySpacingVertical = (colliderBounds.extents.x * 2) / (numberOfRayHorizontal-1) ;
		raySpacingHorizontal = (colliderBounds.extents.y * 2) / (numberOfRayVertical-1) ;
	}

	public void Move(Vector2 velocity){
		CalculateBounds ();
		collisionInfo.Reset ();
		if (velocity.x != 0) {
			RayCastHorizontal (ref velocity);
		}
		if (velocity.y != 0) {
			RayCastVertical (ref velocity);
		}
		transform.position += (Vector3)velocity;
	}

	private void RayCastHorizontal (ref Vector2 velocity){
		float direction = Mathf.Sign (velocity.x);
		float distance = Mathf.Abs (velocity.x);

		Vector2 raycastBase = direction < 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

		for (int i = 0; i < numberOfRayHorizontal; i++) {
			Vector2 raycastOrigin = raycastBase + raySpacingHorizontal * i * Vector2.up;

			RaycastHit2D hit = Physics2D.Raycast (raycastOrigin,new Vector2(direction,0),distance + skinWidth,collisionMask);
			Debug.DrawRay (raycastOrigin,new Vector2(velocity.x,0),Color.red,Time.fixedDeltaTime, false);
			if (hit) {
				velocity.x = (hit.distance- skinWidth) * direction ;
				collisionInfo.collideLeft = direction < 0;
				collisionInfo.collideRight = direction > 0;
			}

			RaycastHit2D hitWall = Physics2D.Raycast (raycastOrigin,new Vector2(direction,0),distance + skinWidth,wallCollisionMask);
			if (hitWall) {
				velocity.x = (hitWall.distance- skinWidth) * direction ;
				collisionInfo.collideWallLeft = direction < 0;
				collisionInfo.collideWallRight = direction > 0;	
			}
		}
	}

	private void RayCastVertical (ref Vector2 velocity){
		float direction = Mathf.Sign (velocity.y);
		float distance = Mathf.Abs (velocity.y);

		Vector2 raycastBase = direction < 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

		for (int i = 0; i < numberOfRayVertical; i++) {
			Vector2 raycastOrigin = raycastBase + raySpacingVertical * i * Vector2.right;

			RaycastHit2D hit = Physics2D.Raycast (raycastOrigin,new Vector2(0,direction),distance + skinWidth,collisionMask);
			Debug.DrawRay (raycastOrigin,new Vector2(0,velocity.y),Color.red,Time.fixedDeltaTime, false);
			if (hit) {
				velocity.y = (hit.distance - skinWidth) * direction;
				collisionInfo.collideBottom = direction < 0;
				collisionInfo.collideTop = direction > 0;
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	 struct RaycastOrigins{
		public Vector2 topLeft,topRight;
		public 	Vector2 bottomLeft,bottomRight;

	}

	public struct CollisionInfo{
		public bool collideTop,collideBottom;
		public bool collideLeft,collideRight,collideWallLeft,collideWallRight;

		public void Reset(){
			collideTop = collideBottom = collideLeft = collideRight = false;
		}
	}
}
