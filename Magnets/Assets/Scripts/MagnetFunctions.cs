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
	public bool freeMetal;
	public bool platformMetal;

	//Private Variables
	private RaycastHit2D hit; //Holds the object the Linecast hits
	private Vector2 force; //Holds the force that the object will be pushed
	private Vector2 tempVel;

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


		//Cast line from player to mouse cursor to try to find metal objects
		if (Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f)) 
		{
			hit = Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f);
			distance = Vector3.Distance (transform.position, hit.transform.position);
			tempVel = hit.rigidbody.velocity;
			freeMetal = true;
			platformMetal = false;
		}
		else if (Physics2D.Linecast (transform.position, pos, layerMask2, -1.0f, 15.0f)) 
		{
			hit = Physics2D.Linecast (transform.position, pos, layerMask2, -1.0f, 15.0f);
			distance = Vector3.Distance (transform.position, hit.transform.position);
			freeMetal = false;
			platformMetal = true;
		}

			
		if (freeMetal && !platformMetal) 
		{
			force = new Vector2 (7.5f - (transform.position.x - hit.transform.position.x), -20 * (transform.position.y - hit.transform.position.y));
			if (Mathf.Abs (transform.position.x - hit.transform.position.x) < 0.8f) 
				force.x = 0f;
			if (Mathf.Abs (transform.position.y - hit.transform.position.y) < 1.0f) 
				force.y = 0f;
			angle = (Mathf.PI * Vector2.Angle (new Vector2 (1, 0), hit.transform.position - transform.position)) / 180;
			force = new Vector2 (-5 * Mathf.Cos (angle), 5 * Mathf.Sin (angle));

			if (Input.GetMouseButton (0) && Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f) && distance < range)
			{
				if (transform.position.y > hit.transform.position.y)
					force.y = 4 * force.y;
				hit.rigidbody.AddForce (force);
				if (Mathf.Abs (transform.position.x - hit.transform.position.x) < 0.25f)
					tempVel.x = 0f;
				if (Mathf.Abs (transform.position.y - hit.transform.position.y) < 0.25f)
					tempVel.y = 0f;
				hit.rigidbody.velocity = tempVel;
			} 

			else if (Input.GetMouseButton (1) && Physics2D.Linecast (transform.position, pos, layerMask1, -1.0f, 15.0f) && distance < range) 
			{
				if (transform.position.y < hit.transform.position.y)
					force.y = 4 * force.y;
				hit.rigidbody.AddForce (-force);
				if (Mathf.Abs (transform.position.x - hit.transform.position.x) < 0.25f)
					tempVel.x = 0f;
				if (Mathf.Abs (transform.position.y - hit.transform.position.y) < 0.25f)
					tempVel.y = 0f;
				hit.rigidbody.velocity = tempVel;
			}
		}
	}


	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll == hit.collider)
		{
			if (freeMetal && !platformMetal) {
				Vector2 x = new Vector2 (0, 0);
				hit.rigidbody.velocity = x;
			}
		}
	}
}

