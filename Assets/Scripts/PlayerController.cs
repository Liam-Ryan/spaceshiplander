using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float thrust;
	public float maxVelocity;
	public float gravity;
	public float maxCollisionVelocity;
	public float coefficientOfRestitution;
	public Bounds levelBoundary;
	public float rotationSpeed;
	public int startingFuel;
	public float minBounceVelocity;

	private Vector3 velocity;
	private Vector3 gravityVector;
	private float halfHeight;
	private Vector3 move;
	private Vector3 front;
	private int remainingFuel;

	private void Start() {
		gravityVector = new Vector3(0, -gravity, 0);
		velocity = new Vector2(0, 0);
		halfHeight = gameObject.GetComponent<SpriteRenderer>().bounds.extents.y;
		remainingFuel = startingFuel;
	}

	private void Update() {
		var verticalInput = Input.GetAxis("Vertical");
		move.y = verticalInput >= 0 ? verticalInput : 0;
		move.x = Input.GetAxis("Horizontal");

//		Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(0, halfHeight, 0));

		var rotationThisUpdate = -(rotationSpeed * move.x * 100 * Time.deltaTime);
		if (Mathf.Abs(move.x) > Mathf.Epsilon) {
			transform.Rotate(0f, 0f, rotationThisUpdate);
		}
		
		if (move.y > 0) {
			if (remainingFuel > 0) {
				velocity += move.y * Time.deltaTime * thrust * transform.up;
				remainingFuel--;
			}
		}

		velocity += Time.deltaTime * gravity * gravityVector;
		velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
	}

	private void FixedUpdate() {
		var pos = transform.position + velocity;
		var unclampedPos = pos;
		pos.x = Mathf.Clamp(pos.x, -levelBoundary.extents.x, levelBoundary.extents.x);
		pos.y = Mathf.Clamp(pos.y, -levelBoundary.extents.y, levelBoundary.extents.y);
		if (!pos.Equals(unclampedPos)) {
			if (!Mathf.Approximately(pos.x, unclampedPos.x))
				velocity.x = 0;
			if (!Mathf.Approximately(pos.y, unclampedPos.y))
				velocity.y = 0;
		}
		transform.position = pos;
	}

	private void Die() {
		Debug.Log("Dead!");
	}

	private void LevelComplete() {
		Debug.Log("Victory!");
	}

	private bool IsTooFast() {
		Debug.LogFormat("Velocity magnitude is {0}", velocity.magnitude);
		return velocity.magnitude > maxCollisionVelocity;
	}

	private void BounceOffGeometry(Collision2D other) {
		var bounceDirection = other.transform.up;
		var bounceFactor = velocity.magnitude < minBounceVelocity ? 1 : coefficientOfRestitution;
		
		if (Mathf.Approximately(bounceDirection.x, 1)) {
			velocity.x *= -1 * bounceFactor;
		}
		if (Mathf.Approximately(bounceDirection.y, 1)) {
			velocity.y *= -1 * bounceFactor;
		}
		if(velocity.y < Mathf.Epsilon)
//		velocity.x *= -1;
//		velocity.y *= -1;
		Debug.LogFormat("collision position is {0}", other.transform.position);
		Debug.LogFormat("player positoin is {0}", transform.position);
		Debug.LogFormat("relative direction is {0}", other.transform.position - transform.position);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		Debug.Log("Collision!");
		Debug.LogFormat("Collided with {0}", other.gameObject.name);
		Debug.LogFormat("collision position is {0}", other.transform.position);
		Debug.LogFormat("player positoin is {0}", transform.position);
		Debug.LogFormat("relative direction is {0}", other.transform.position - transform.position);
		Debug.LogFormat("Up for the collision object is {0}", other.transform.up);

		if (IsTooFast()) {
			Die();
		} else {
			if (other.gameObject.CompareTag("Finish")) {
				LevelComplete();
			} else {
				BounceOffGeometry(other);
			}
		}
	}

	private void OnCollisionStay2D(Collision2D other) {
		var collisionTime = Time.deltaTime;
		if (collisionTime > 5) {
			Debug.Log("collision timer reached");
		}
		Debug.LogFormat("Colliding with {0}", other.gameObject.name);
	}
}