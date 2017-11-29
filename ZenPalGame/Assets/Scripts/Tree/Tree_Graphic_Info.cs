using UnityEngine;
using System.Collections;

public class Tree_Graphic_Info : MonoBehaviour {

	public GameObject PrevSegment;
	public Vector3 graphicPosUpdate;
	public float graphicAngle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		graphicPosUpdate = transform.position;
	}
}
