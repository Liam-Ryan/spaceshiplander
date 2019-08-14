using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    public float thrust;

    public float rotationSpeed;
    private Vector2 move;
    private Rigidbody2D body;
    
    // Start is called before the first frame update
    private void Start() {
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
        var verticalInput = Input.GetAxis("Vertical");
        move.y = verticalInput >= 0 ? verticalInput : 0;
        move.x = Input.GetAxis("Horizontal");
        if (move.x != 0) {
            Debug.LogFormat("Rotation is {0}", body.rotation);
            body.MoveRotation(body.rotation - (rotationSpeed * move.x * 100 * Time.deltaTime));
            Debug.LogWarningFormat("Rotation after adjustment is {0}", body.rotation);
        }

//        body.MovePosition();
        /*if (verticalInput > 0) 
            Debug.LogFormat("verticalInput is {0}", verticalInput);
        if(Mathf.Abs(move.x) > Mathf.Epsilon)
            Debug.LogFormat("horizontalInput is {0}", move.x);*/
    }
}
