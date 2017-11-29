using UnityEngine;
using System.Collections;

public class Tree_Stage_Class {

	public enum TreeSegmentType
	{
		ROOT,
		TRUNK,
		BRANCH
	};

	public enum TreeStage
	{
		PUASED,
		GROWING,
		SPURT
	};
	
	
	public TreeSegmentType treeSegmentTypes;
	public TreeStage treeStages;

}
