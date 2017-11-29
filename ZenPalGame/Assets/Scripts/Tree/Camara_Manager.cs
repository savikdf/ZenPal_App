using UnityEngine;
using System.Collections;

public class Camara_Manager : MonoBehaviour {

	public GameObject camfollowPoint;
	Camera curCam;
	Vector2 topBranchPos;
	Vector3 startPos;
	public float offset, maxOrthSize, minOrthSize;
	// Use this for initialization
	void Awake () 
	{
		curCam = GetComponent<Camera>();
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(Tree_Position_Manager.topBranch != null)
		{


		topBranchPos = new Vector2 (Tree_Position_Manager.topBranch.transform.position.x, Tree_Position_Manager.topBranch.transform.position.y - offset);
			maxOrthSize = topBranchPos.y * 2;

			if(curCam.orthographicSize >= minOrthSize && curCam.orthographicSize <= maxOrthSize)
			{
				curCam.orthographicSize = curCam.orthographicSize + topBranchPos.y/1200;
			}
			else if(curCam.orthographicSize <= minOrthSize)
			{
				curCam.orthographicSize = minOrthSize;
			}
			else if( curCam.orthographicSize >= maxOrthSize)
			{
				curCam.orthographicSize = maxOrthSize;
			}
		curCam.transform.position = Vector3.Lerp(transform.position, new Vector3( startPos.x, startPos.y + topBranchPos.y, startPos.z), .005f);
	}
	}
}
