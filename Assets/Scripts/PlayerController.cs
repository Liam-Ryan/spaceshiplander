using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float thrust;
	public float maxVelocity;
	public float gravity;
	public float maxCollisionVelocity;
	
	private Vector3 velocity;
	private readonly Vector3 gravityVector = new Vector3(0, -0.1f, 0);

	public float rotationSpeed;
	private Vector3 move;

	private void Start() {
		velocity = new Vector2(0, 0);
	}
	
	private void Update() {
		var verticalInput = Input.GetAxis("Vertical");
		move.y = verticalInput >= 0 ? verticalInput : 0;
		move.x = Input.GetAxis("Horizontal");
		
		var rotationThisUpdate = -(rotationSpeed * move.x * 100 * Time.deltaTime);
		if (Mathf.Abs(move.x) > Mathf.Epsilon) {
			transform.Rotate(0f, 0f, rotationThisUpdate);
		}

		if (move.y > 0) {
			velocity += move.y * Time.deltaTime * thrust * transform.up;
		}

		velocity += Time.deltaTime * gravity * gravityVector;
		velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
	}

	private void FixedUpdate() {
		transform.position += velocity;
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
		Debug.Log("Bounce");
	}

	private void OnCollisionEnter2D(Collision2D other) {
		Debug.Log("Collision!");
		Debug.LogFormat("Collided with {0}", other.gameObject.name);
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
	
}