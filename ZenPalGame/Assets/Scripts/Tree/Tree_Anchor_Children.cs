using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tree_Anchor_Children : MonoBehaviour {

	Tree_Segment_Script treeSegInfo;
	PositionManager anchorInfo;
	public List<PositionManager> childAnchorSegments = new List<PositionManager>();

	// Use this for initialization
	void Start () 
	{
		anchorInfo = GetComponent<PositionManager>();
		treeSegInfo = anchorInfo.curChild.GetComponent<Tree_Segment_Script>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(treeSegInfo.treeSegmentTypes == Tree_Segment_Script.TreeSegmentType.BRANCH && treeSegInfo.isMasterBranch)
		{
				childAnchorSegments = GetComponentsInChildren<PositionManager>().ToList();
		}
	}
}
