using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float jumpForce;
	private Rigidbody2D rb;
	public bool isGrounded;
	public bool rightward = true;
	public bool leftward = false;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		speed = 3.0f;
		jumpForce = 500.0f;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.D)) 
		{
			transform.Translate (speed * Time.deltaTime, 0, 0);
			rightward = true;
			leftward = false;
		} 
		else if (Input.GetKey (KeyCode.A)) {
			transform.Translate (speed * Time.deltaTime, 0, 0);
			rightward = false;
			leftward = true;
		}
			

		Vector3 temp = transform.eulerAngles;
		if (rightward && !leftward)
			temp.y = 0.0f;
		else if (leftward && !rightward)
			temp.y = 180.0f;
		transform.eulerAngles = temp;

		int layerMask = (1 << MagnetFunctions.platformMetalMask);

		if (Input.GetKeyDown (KeyCode.W) && Physics2D.Raycast (transform.position - new Vector3 (0, 1, 0), Vector2.down, 0.1f, layerMask))
			Jump ();
	}

	void Jump ()
	{
		rb.AddForce (Vector2.up * jumpForce);
		Debug.Log ("Jump");
	}
}