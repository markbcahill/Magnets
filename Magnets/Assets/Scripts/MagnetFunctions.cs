using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetFunctions : MonoBehaviour 
{
	//Static Variables
	//Holds the interger values of the layer masks for free metal and metal platforms for Raycasts and Linecasts
	static public int freeMetalMask;
	static public int platformMetalMask;

	//Public variables
	public float range = 7.5f; //How far away the magnet can attract/repulse objects
	public float distance; //Holds distance between player and objected that is targeted
	public float angle; //Holds angle between player and targeted object 

	//Private Variables
	private RaycastHit2D hit; //Holds the object the Linecast hits
	private Vector2 force; //Holds the force that the object will be pushed
	private Vector2 move;

	void Awake () 
	{
		//Setting Values for layer masks
		freeMetalMask = LayerMask.NameToLayer ("FreeMetal");
		platformMetalMask = LayerMask.NameToLayer ("PlatformMetal");
		//tempAngle = 0;
	}

	void Update ()
	{
		//Temporary values to use in linecast
		int layerMask1 = (1 << freeMetalMask);
		int layerMask2 = (1 << platformMetalMask);

		//Finding position of mouse cursor
		Vector3 temp = Input.mousePosition;
		temp.z = 10.0f;
		Vector3 pos = Camera.main.ScreenToWorldPoint (temp);
		Debug.DrawLine (transform.position, pos, Color.red);
		//Debug.Log (Mathf.Sin((Mathf.PI * tempAngle)/180));


		//Cast line from player to mouse cursor to try to find metal objects
		if (Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f))
			hit = Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f);
		else if (Physics2D.Linecast (transform.position, pos, layerMask2, -1.0f, 15.0f))
			hit = Physics2D.Linecast (transform.position, pos, layerMask2, -1.0f, 15.0f);
		
		distance = Vector3.Distance (transform.position, hit.transform.position);
		Vector2 tempVel = hit.rigidbody.velocity;

		force = new Vector2 (7.5f - (transform.position.x - hit.transform.position.x), -20 * (transform.position.y - hit.transform.position.y));
		move = Time.deltaTime  * 2 * new Vector2 (transform.position.x - hit.transform.position.x, 10*(transform.position.y - hit.transform.position.y));

		if (Mathf.Abs (transform.position.x - hit.transform.position.x) < 0.8f) 
		{ 
			force.x = 0f;
			move.x = 0f;
		}
		if (Mathf.Abs (transform.position.y - hit.transform.position.y) < 1.0f)
		{
			force.y = 0f;
			move.y = 0f;
		}



		angle = (Mathf.PI * Vector2.Angle (new Vector2(1,0), hit.transform.position-transform.position))/180;
		Debug.Log (angle);
		force = new Vector2 (-5 * Mathf.Cos (angle), 10 * Mathf.Sin (angle));
		//if (transform.position.y > hit.transform.position.y)
		//	force.y = -force.y;
		Debug.Log (force);


		if (Input.GetMouseButton (0) && Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f) && distance < range) 
		{
			hit.rigidbody.AddForce (force);
			if (Mathf.Abs (transform.position.x - hit.transform.position.x) < 0.25f)
				tempVel.x = 0f;
			if (Mathf.Abs (transform.position.y - hit.transform.position.y) < 0.25f)
				tempVel.y = 0f;
		} 
		else if (Input.GetMouseButton (1) && Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f) && distance < range) 
		{
			hit.rigidbody.AddForce (-force);
			if (Mathf.Abs (transform.position.x - hit.transform.position.x) < 0.25f)
				tempVel.x = 0f;
			if (Mathf.Abs (transform.position.y - hit.transform.position.y) < 0.25f)
				tempVel.y = 0f;
		}

		hit.rigidbody.velocity = tempVel;

		if (Input.GetKey (KeyCode.E) && Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f) && distance < range)
			hit.transform.Translate (move);
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll == hit.collider)
		{
			Debug.Log ("Hit by metal");
		}
	}
}
