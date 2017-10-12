using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpForce;
	private Rigidbody2D rb;
	public bool jumping;
	public float yNow;
	// Use this for initialization
	void Start () {
		speed = 2.0f;
		jumpForce = 425.0f;
		jumping = false;
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		float yThen = yNow;
		yNow = transform.position.y;
		float diff = yNow - yThen; 
		if(Input.GetKey(KeyCode.D))
			transform.Translate (speed * Time.deltaTime, 0, 0);
		else if(Input.GetKey(KeyCode.A))
			transform.Translate (-speed * Time.deltaTime, 0, 0);
		if (Input.GetKeyDown (KeyCode.W) && diff < ) {
			rb.AddForce (transform.up * jumpForce);
			jumping = true;
		}

	}
}
