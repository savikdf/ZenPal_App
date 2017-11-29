using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tree_Segment_Script : MonoBehaviour {

	[HideInInspector]
	public GameObject branchSegment, masterSegment, masterSegGroup;
	[HideInInspector]
	public Vector3 startPos, prevStartPos, updatePos;
	[HideInInspector]
	public Tree_Master master;
	[HideInInspector]
	public Segment_GrowthState growthMan;
	[HideInInspector]
	public Transform root, prevSeg, pos;
	[HideInInspector]
	public Vector2 scale;
	[HideInInspector]
	public bool nextSegmentMade, randomSegment, isMasterBranch, isTree, createBranch;
	[HideInInspector]
	public float angle, growthMax, growthRate, inputAngle; 
	[HideInInspector]
	public int depth;

	public enum TreeSegmentType
	{
		ROOT,
		TRUNK,
		BRANCH,
		TREE
	};
	
	public TreeSegmentType treeSegmentTypes;

	void Awake () 
	{	
		growthMan = GetComponent<Segment_GrowthState>();
		growthMan.curSegment = this.gameObject;

	}

	void Start()
	{


		if(treeSegmentTypes == TreeSegmentType.TRUNK || treeSegmentTypes == TreeSegmentType.ROOT)
		{
			Tree_Master.treeTrunkList.Add(this.gameObject);
		}else
		{ 
		}

		if(treeSegmentTypes == TreeSegmentType.TREE)
		{
			isTree = true;
			masterSegment = this.gameObject;
		}


		if(depth == 1 && treeSegmentTypes == TreeSegmentType.BRANCH || treeSegmentTypes == TreeSegmentType.ROOT)
		{
			masterSegGroup = new GameObject();
			masterSegGroup.name = "masterSegGroup" + "masterSegment" + depth;
			masterSegGroup.transform.parent = pos;
			pos.parent = Tree_Master.StartSeg.transform;
			masterSegGroup.transform.position = startPos;
			masterSegGroup.AddComponent<Tree_Master_SegmentGroup_Info>();
			Tree_Master.segmentMasters.Add(masterSegGroup.gameObject);

			transform.parent = masterSegGroup.transform;

			isMasterBranch = true;
			masterSegment = this.gameObject;
		}
	}
	
	
	
	void Update () 
	{
	}
	
}

