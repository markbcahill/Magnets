using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour 
{
	public GameObject player;
	// Use this for initialization
	void Awake () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = player.transform.position + new Vector3 (0, 0, -10);
	}
}
