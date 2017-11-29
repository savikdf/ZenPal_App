using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TreeParms {

	public static float branchAngle = 20.0f;       //how far left/right each branch goes from its parent.
	public static float branchStartAngle = 60f;
	public static float branchScaleIncrease = 3f;


	public static float trunkAngle = 5.0f;       //how far left/right each Trunk goes from its parent.
	public static float inputAngle = 5.0f;       //how far left/right each Trunk goes from its parent.

	public static float minScale = 0.1f;          //what's the minimum scale of each branch?
	public static float minScaleX = 0.01f;          //what's the minimum scale of each branch?

	public static float scaleChange = .02f;       //how much smaller than its parent is each branch?
	public static float angleRandom = 5.0f;        //how much random variation is there in the angle for each branch?
	public static float scaleRandom = 0.01f;        // how much random variation is there in the scale for each branch?

	public static float growthDim = .05f;

	public static int maxDepth = 12;               //what's the maximum "depth" of the recursive algorithm?

	public static int splitChokePoint = 2; //How many segments are allowed to split at any given time?
	public static int splitIndex = 0; //Current Number of Segments splitting/About to split

	public static Vector2 growthRate = new Vector3 (2f,20f); //Rate of Growth
	public static Vector2 growthSplitPoint = new Vector3 (.2f,.4f); // Max of X and Y Scales before increase in Size (X currently not set)

	public static Vector2 globalGrowth = new Vector3 (.05f,15f); //Rate of Growth Trought the Entire Tree
	public static Vector2 globalSplit = new Vector3 (1.25f,2f); // After Spurt growth, check to create new segments if needed

	public static Vector3 trunkTopOffset = new Vector3(0, 1.8f, 0);
	
	public static void CreateSegment(Vector3 startPosTemp, Vector3 prevStartPosTemp, float angleTemp, Vector2 scaleTemp, int depthTemp, string name, GameObject curObj, GameObject treeMast, GameObject spawnObj, Tree_Segment_Script.TreeSegmentType treeinfo)
	{
		Quaternion rot = Quaternion.Euler( 0, 0, angleTemp);
	

		Tree_Segment_Script curTreeSeg = curObj.GetComponent<Tree_Segment_Script>();

		if(!curTreeSeg.GetComponent<Segment_GrowthState>().hitEnd && !curTreeSeg.GetComponent<Tree_Segment_Script>().isMasterBranch && curTreeSeg.GetComponent<Segment_GrowthState>().treeStages == Segment_GrowthState.TreeStage.GROWING)
		{

			curTreeSeg.masterSegment.GetComponent<Tree_Segment_Script>().masterSegGroup.GetComponent<Tree_Master_SegmentGroup_Info>().SegmentsList.Add(curTreeSeg.gameObject);

			curTreeSeg.transform.parent = curTreeSeg.masterSegment.GetComponent<Tree_Segment_Script>().masterSegGroup.transform;


			if(curTreeSeg.pos != null)
			{
				curTreeSeg.pos.GetComponent<PositionManager>().Explode();
				curTreeSeg.pos = null;
			}
			
			Tree_Master.growthList.Remove(curTreeSeg.gameObject);

			
		}


		if(scaleTemp.y <= TreeParms.minScale && curObj.GetComponent<Segment_GrowthState>().treeStages == Segment_GrowthState.TreeStage.GROWING || 
		   scaleTemp.y >= curObj.GetComponent<Segment_GrowthState>().growthSplitPoint.y && curObj.GetComponent<Segment_GrowthState>().treeStages == Segment_GrowthState.TreeStage.SPURT)
		{
			
			if(depthTemp >= maxDepth && curTreeSeg.treeSegmentTypes == Tree_Segment_Script.TreeSegmentType.BRANCH)
			{
				CreateLeaf(curTreeSeg.angle, curTreeSeg.transform.position, curObj);
				Tree_Master.growthList.Remove(curTreeSeg.gameObject);
				return;
			}
			CreateLeaf(curTreeSeg.angle, curTreeSeg.transform.position, curObj);
			curObj.GetComponent<Segment_GrowthState>().hitEnd = true;			
			return;         //done with this 'leaf'!
		}


		GameObject obj = (GameObject)GameObject.Instantiate(spawnObj, startPosTemp, rot);
		obj.name = name;


		GameObject anchrObj = (GameObject)GameObject.Instantiate(Tree_Master.anchorPoint, startPosTemp, rot);

		PositionManager anchor = anchrObj.GetComponent<PositionManager>();

		if(depthTemp == 1 && treeinfo == Tree_Segment_Script.TreeSegmentType.BRANCH)
		{
			anchrObj.transform.name = "MASTER_Anchor" + depthTemp +" "+ obj.name;
		}else{
		anchrObj.transform.name = "Anchor" + depthTemp +" "+ obj.name;
		}
	

		Tree_Segment_Script segMan = obj.GetComponent<Tree_Segment_Script>();

		anchor.startPosTemp = startPosTemp;
		anchor.angleTemp = angleTemp;
		anchor.prevSegment = curObj.transform;
		anchor.curChild = obj.transform;
		anchor.trunkTopOffset = trunkTopOffset;


		segMan.startPos = startPosTemp;
		segMan.angle = angleTemp;
		segMan.scale = new Vector2 (scaleTemp.x, scaleTemp.y);
		segMan.depth = depthTemp;
		segMan.masterSegment = curObj.GetComponent<Tree_Segment_Script>().masterSegment;

		segMan.master = treeMast.GetComponent<Tree_Master>();
		segMan.pos = anchrObj.transform;
		segMan.prevSeg = curObj.transform;

		segMan.GetComponentInChildren<SpriteRenderer>().sortingOrder = -depthTemp;

		//Current Branches Position and Rotation Point
		segMan.transform.parent = anchrObj.transform;
		anchrObj.transform.parent = Tree_Master.StartSeg.transform;
			if(treeinfo == Tree_Segment_Script.TreeSegmentType.BRANCH)
			{
			segMan.treeSegmentTypes = Tree_Segment_Script.TreeSegmentType.BRANCH;
			segMan.transform.name = "Branch" + "_" + depthTemp;
			}
			else if(treeinfo == Tree_Segment_Script.TreeSegmentType.TRUNK)
			{
			segMan.treeSegmentTypes = Tree_Segment_Script.TreeSegmentType.TRUNK;
			segMan.transform.name = "Trunk" + "_" + depthTemp;
			}

           // Tree_Master.AddSegment(obj);
			Tree_Master.growthList.Add(obj);
			Tree_Master.allTreeSegment.Add(obj);
			Tree_Master.anchorList.Add(anchrObj);

	
		//Control of Current Segments Growth
        Segment_GrowthState growthState = obj.GetComponent<Segment_GrowthState>();
        growthState.growthSplitPoint = new Vector2(scaleTemp.x, scaleTemp.y);
        growthState.prevSeg = curObj.transform;
        growthState.depth = depthTemp;
	}

	public static void CreateLeaf(float angle, Vector3 startPos, GameObject thisOBJ)
	{
		Quaternion rot = Quaternion.Euler( 0, 0, angle);
		GameObject obj = (GameObject)GameObject.Instantiate(Tree_Master.leafPref, startPos, rot);
		obj.transform.parent = thisOBJ.transform;
	}

}
