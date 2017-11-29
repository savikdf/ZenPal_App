using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tree_Position_Manager : MonoBehaviour {

	public List<GameObject> TreeSegment = new List<GameObject>();
	public List<GameObject> anchorList = new List<GameObject>();

	public List<GameObject> segmentMasters = new List<GameObject>();

	public static GameObject topBranch;

	// Use this for initialization
	void Awake () 
	{
		TreeSegment = Tree_Master.allTreeSegment;
		anchorList = Tree_Master.anchorList;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Tree_Master.treeTrunkList.Count != 0)
		{
		topBranch = Tree_Master.treeTrunkList[(CustomExtensions.GetHighestPoint(Tree_Master.treeTrunkList))];
		}

		float wiggle = Mathf.Sin(Time.time) * 2;

		//Dynamic Angle Placement of Current Segments Based on User Input
		if(Tree_Master.allTreeSegment.Count != 0)
		{
		for(int i = 0; i <  Tree_Master.allTreeSegment.Count; i++)
		{

			Tree_Segment_Script trunk =  Tree_Master.allTreeSegment[i].GetComponent<Tree_Segment_Script>();

			if(trunk != null && trunk.GetComponent<Tree_Segment_Script>().masterSegment != null)
			{
			if(trunk.GetComponent<Tree_Segment_Script>().masterSegment.GetComponent<Tree_Segment_Script>().masterSegGroup != null)
			{
				GameObject masterSegGroup =	trunk.GetComponent<Tree_Segment_Script>().masterSegment.GetComponent<Tree_Segment_Script>().masterSegGroup;

				if(trunk.GetComponent<Segment_GrowthState>().hitEnd)
				{
					trunk.updatePos = CustomExtensions.TrunkPositionUpdate(trunk.angle, new Vector2 (trunk.transform.localScale.x, trunk.transform.localScale.y), trunk.transform);
				}
				else if(trunk.transform.parent == masterSegGroup.transform)
				{
					trunk.updatePos = CustomExtensions.TrunkPositionUpdate(trunk.angle, new Vector2 (trunk.transform.localScale.x *  masterSegGroup.transform.localScale.x, 
					                                                                                 trunk.transform.localScale.y *  masterSegGroup.transform.localScale.y), trunk.transform);
				}
			}
			else
			{
					trunk.updatePos = CustomExtensions.TrunkPositionUpdate(trunk.angle, new Vector2 (trunk.transform.localScale.x, trunk.transform.localScale.y), trunk.transform);
			}
	

			if(trunk.pos != null && trunk.transform.parent != trunk.GetComponent<Tree_Segment_Script>().masterSegment.GetComponent<Tree_Segment_Script>().masterSegGroup.transform)
			{
				trunk.transform.position = trunk.pos.position;
			}
			else if(trunk.prevSeg != null)
			{
				trunk.transform.position = trunk.prevSeg.GetComponent<Tree_Segment_Script>().updatePos;
			}


			trunk.inputAngle = Tree_Input_Manager.treeInputAngle;
			Quaternion rot = new Quaternion ();


			if(trunk.treeSegmentTypes == Tree_Segment_Script.TreeSegmentType.TRUNK)
			{
				rot = Quaternion.Euler( 0, 0, trunk.angle + (Tree_Input_Manager.treeInputAngle/4) * trunk.depth + wiggle );
				rot = Quaternion.Lerp (trunk.transform.rotation, rot, 0.05f);
				trunk.transform.rotation = rot;
			}
			else if(trunk.isMasterBranch && trunk.pos != null &&  trunk.prevSeg != null)
			{
				rot = Quaternion.Euler( 0, 0, trunk.angle + trunk.prevSeg.GetComponent<Tree_Segment_Script>().angle + (Tree_Input_Manager.treeInputAngle/2) * trunk.depth + wiggle );
				rot = Quaternion.Lerp (trunk.pos.transform.rotation, rot, 0.05f);
				trunk.pos.transform.rotation = rot;
			}
			}
		}
		}
		

		if(Tree_Master.anchorList.Count != 0)
		{
			for(int i = 0; i <  Tree_Master.anchorList.Count; i++)
		{
			PositionManager posMan =  Tree_Master.anchorList[i].GetComponent<PositionManager>();
			if(posMan.prevSegment != null)
			{

			posMan.transform.position  = CustomExtensions.AdjustPosition(posMan.prevSegment);

			}
			if(anchorList.Count > 1)
			{
				
			}
		}
		}

	}

}
