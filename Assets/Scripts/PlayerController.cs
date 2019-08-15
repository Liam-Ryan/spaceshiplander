using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float thrust;
	public float maxVelocity;
	private Vector3 velocity;

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
			velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
		}
	}

	private void FixedUpdate() {
		transform.position += velocity;
	}
}