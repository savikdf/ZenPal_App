using UnityEngine;
using System.Collections;

public class PositionManager : MonoBehaviour {
	[HideInInspector]
	public Vector3 startPosTemp, tempPos, trunkTopOffset;
	[HideInInspector]
	public float angleTemp;
	[HideInInspector]
	public Vector2 scaleTemp;

	public Transform prevSegment, curChild;
	

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	

	}

	public void Explode()
	{
		Tree_Master.anchorList.Remove(this.gameObject);
		GameObject.Destroy(this.gameObject);
	}

}
