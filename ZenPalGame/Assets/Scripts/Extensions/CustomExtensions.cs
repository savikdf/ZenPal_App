using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class CustomExtensions {

	public static float ClampAngle(float n_TempAngle)
	{
		//Clamping Anges, Mathf.Clamp would work too
		//is it a positive angle or a negetive angle
		float rot = n_TempAngle;

	//	rot = Mathf.Clamp(n_TempAngle,15, 90);

		return rot;
	}

	public static Vector3 TrunkPositionUpdate(float n_angleTemp, Vector2 n_scaleTemp, Transform curSeg)
	{
		Quaternion rot = Quaternion.Euler( 0, 0, curSeg.rotation.eulerAngles.z);
		Vector3 n_updatePos = new Vector3(curSeg.position.x, curSeg.position.y , curSeg.position.z) + (rot * (TreeParms.trunkTopOffset * (n_scaleTemp.y)));
		return n_updatePos;
	}
	
	public static Vector3 AdjustPosition(Transform pars)
	{
		Vector3 pos;
		Tree_Segment_Script parsSegInfo = pars.GetComponent<Tree_Segment_Script>();
		pos = new Vector3 (parsSegInfo.updatePos.x, parsSegInfo.updatePos.y, 0);
		return pos;	
	}

	public static int GetHighestPoint(this List<GameObject> list)
	{
		float maxPos;
		int maxIndex;
		int size = list.Count;

		if(size < 2 || list.Count == 0 || list[0] == null)
			return 0; 
		
		maxPos = list[0].transform.position.y;
		maxIndex = 0;

		for(int i = 1; i < size; i++)
		{
			float thisPos = list[i].transform.position.y;
			if(thisPos > maxPos)
			{
				maxPos = thisPos;
				maxIndex = i;
			}
		}

		return maxIndex;
	}

	public static Vector3 Scale(float growthRate)
	{
		Vector3 tempVec = new Vector3(0,0,0);
		tempVec.x = growthRate;
		tempVec.y = growthRate;
		tempVec.z = growthRate;
		return tempVec;
	}
}
