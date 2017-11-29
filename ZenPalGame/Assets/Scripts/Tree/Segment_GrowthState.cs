using UnityEngine;
using System.Collections;

public class Segment_GrowthState : MonoBehaviour {
	public Vector2 growthSplitPoint;

	[HideInInspector]
	public Transform prevSeg;
	[HideInInspector]
	public Tree_Master master;

	public bool split, hitEnd;
	[HideInInspector]
	public GameObject curSegment;
	[HideInInspector]
	public int depth;
 	
	public enum TreeStage
	{
		PUASED,
		GROWING,
		SPURT
	};

	public TreeStage treeStages;


	void Start () 
	{	
		if(curSegment.GetComponent<Tree_Segment_Script>() != null && curSegment.GetComponent<Tree_Segment_Script>().treeSegmentTypes != Tree_Segment_Script.TreeSegmentType.TREE)
		{
			treeStages = TreeStage.SPURT;
		}
	}
	

	void Update () 
	{


	}


}
