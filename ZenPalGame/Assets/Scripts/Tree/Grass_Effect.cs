using UnityEngine;
using System.Collections;

public class Grass_Effect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float wiggle = Mathf.Sin(Time.time) * 4;

		Quaternion rot = new Quaternion();

		rot = Quaternion.Euler( 0, 0, wiggle );
		rot = Quaternion.Lerp (transform.rotation, rot, 5f);
		transform.rotation = rot;
	}
}
