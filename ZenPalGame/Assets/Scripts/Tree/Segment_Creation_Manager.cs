using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Segment_Creation_Manager : MonoBehaviour {

	//public List<GameObject> TreeSegment = new List<GameObject>();
	public List<GameObject> trunkList = new List<GameObject>();
	public List<GameObject> creatorList = new List<GameObject>();
	public List<GameObject> segmentMasters = new List<GameObject>();

	public int creationSplitIndex;
	public GameObject topTrunk;
	public float spawnChance;
	// Use this for initialization
	void Start () 
	{
		creatorList = Tree_Master.creatorListMaster;
		trunkList = Tree_Master.treeTrunkList;
		segmentMasters = Tree_Master.segmentMasters;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(Tree_Master.treeTrunkList.Count != 0)
		{
			topTrunk = Tree_Master.treeTrunkList[(CustomExtensions.GetHighestPoint(Tree_Master.treeTrunkList))];
		}

		if(Tree_Master.creatorListMaster.Count != 0)
		{

			for(int i = 0; i < Tree_Master.creatorListMaster.Count; i++)
			{
			Tree_Segment_Script trunk = Tree_Master.creatorListMaster[i].GetComponent<Tree_Segment_Script>();
				//==========================================
				//Creation of Random Branches
				//==========================================


			if(trunk.GetComponent<Segment_GrowthState>().treeStages == Segment_GrowthState.TreeStage.GROWING && trunk.treeSegmentTypes == Tree_Segment_Script.TreeSegmentType.BRANCH)
			{
					if(Random.value <= spawnChance && !trunk.randomSegment && trunk.depth >= 5)
				{
					trunk.randomSegment = true;
					CreateBranchSegment(trunk.startPos, trunk.angle, new Vector2 ( trunk.scale.x, trunk.scale.y), trunk.depth, trunk.gameObject);
				}
			}
			
				if(trunk.createBranch)
				{
					if(trunk.transform.parent == trunk.GetComponent<Tree_Segment_Script>().masterSegment.GetComponent<Tree_Segment_Script>().masterSegment)
					{
						GameObject masterSeg = trunk.GetComponent<Tree_Segment_Script>().masterSegment.GetComponent<Tree_Segment_Script>().masterSegment;

						CreateBranchSegment(trunk.transform.position, trunk.angle, new Vector2 (trunk.scale.x * masterSeg.transform.localScale.x, 
						                                                                        trunk.scale.y * masterSeg.transform.localScale.y), 0,  trunk.gameObject);
						Tree_Master.creatorListMaster.Remove(trunk.gameObject);
					}
					else
					{
						CreateBranchSegment(trunk.transform.position, trunk.angle, new Vector2 (trunk.scale.x , trunk.scale.y), 0,  trunk.gameObject);
					Tree_Master.creatorListMaster.Remove(trunk.gameObject);
					}
					trunk.createBranch = false;
				}

				if(!trunk.nextSegmentMade)
			{
				
				//==========================================
				//Creates Next Brnach Gen of Tree
				//==========================================
				//Create 2 Branches
				//Create a Branch to the Left
				//Create a Branch to the Right
				//Create Continuation of Current Branch/Trunk
				//No Branch is Created
				trunk.nextSegmentMade = true;
				
				if(trunk.treeSegmentTypes == Tree_Segment_Script.TreeSegmentType.TRUNK || trunk.treeSegmentTypes == Tree_Segment_Script.TreeSegmentType.ROOT)
				{
						CreateTrunkSegment(trunk.transform.position, trunk.angle, new Vector2 (trunk.scale.x, trunk.scale.y), trunk.depth, Tree_Input_Manager.treeInputAngle, trunk.gameObject);
				}
				else
				{
						CreateBranchSegment(trunk.transform.position, trunk.angle, new Vector2 (trunk.scale.x, trunk.scale.y), trunk.depth, trunk.gameObject);
				}
				
			}
			
			Tree_Master.creatorListMaster.Remove(trunk.gameObject);
		}
		}

	}


	public void CreateTrunkSegment(Vector3 n_startPosTemp, float n_angleTemp, Vector2 n_scaleTemp, int n_depthTemp, float n_inputangle, GameObject n_thisOBJ)
	{
	

		Quaternion rot = Quaternion.Euler( 0, 0, n_angleTemp);
		Vector3 topPos = n_startPosTemp + (rot * (TreeParms.trunkTopOffset * (n_scaleTemp.y)));
		
	
			TreeParms.CreateSegment(topPos, n_startPosTemp,
			                        n_angleTemp + n_inputangle,
			                        new Vector2(n_scaleTemp.x, 
			           				n_scaleTemp.y - (TreeParms.scaleChange + Random.Range(0, TreeParms.scaleRandom)) ),
			                        n_depthTemp+1, "Trunk_Segment" + n_depthTemp, n_thisOBJ, Tree_Master.treeMasterGameOBJ, Tree_Master.trunkPrefab, 
			                       	Tree_Segment_Script.TreeSegmentType.TRUNK);
		}


	
	public void CreateBranchSegment(Vector3 n_startPosTemp, float n_angleTemp, Vector2 n_scaleTemp, int n_depthTemp, GameObject n_thisOBJ)
	{

		int Tempint;
		Tempint = Random.Range(1,4);

		Quaternion rot = Quaternion.Euler( 0, 0, n_angleTemp);
		
		Vector3 topPos = n_startPosTemp + (rot * (TreeParms.trunkTopOffset * (n_scaleTemp.y)));
		
		//THE 4 TREE RULES OF ISENGARD
		float angleLeft = CustomExtensions.ClampAngle(n_angleTemp - TreeParms.branchAngle + Random.Range(-TreeParms.angleRandom, TreeParms.angleRandom));
		float angleRight = CustomExtensions.ClampAngle(n_angleTemp + TreeParms.branchAngle + Random.Range(-TreeParms.angleRandom, TreeParms.angleRandom));

		switch (Tempint)
		{
		case 1:
			//Create Left Side
			TreeParms.CreateSegment(topPos, n_startPosTemp,
			                        angleLeft,
			                        new Vector2(n_scaleTemp.x, 
			            			n_scaleTemp.y - (TreeParms.scaleChange + Random.Range(0, TreeParms.scaleRandom)) ),
			                        n_depthTemp+1, "Main_Left_Branch", n_thisOBJ, Tree_Master.treeMasterGameOBJ, Tree_Master.trunkPrefab, 
			                        Tree_Segment_Script.TreeSegmentType.BRANCH);
			break;
			
		case 2:
			//Create Right Side
			TreeParms.CreateSegment(topPos,n_startPosTemp,
			                        angleRight ,
			                        new Vector2(n_scaleTemp.x, 
			            			n_scaleTemp.y - (TreeParms.scaleChange + Random.Range(0, TreeParms.scaleRandom)) ),
			                        n_depthTemp+1, "Main_Right_Branch", n_thisOBJ, Tree_Master.treeMasterGameOBJ, Tree_Master.trunkPrefab, 
			                        Tree_Segment_Script.TreeSegmentType.BRANCH);
			break;
			
		case 3:
			//Create Left Side
			TreeParms.CreateSegment(topPos,n_startPosTemp,
			                        angleLeft ,
			                        new Vector2(n_scaleTemp.x, 
			           				n_scaleTemp.y - (TreeParms.scaleChange + Random.Range(0, TreeParms.scaleRandom)) ),
			                        n_depthTemp+1, "Twin_Left_Branch", n_thisOBJ, Tree_Master.treeMasterGameOBJ, Tree_Master.trunkPrefab, 
			                        Tree_Segment_Script.TreeSegmentType.BRANCH);
			
			//Create Right Side
			TreeParms.CreateSegment(topPos,n_startPosTemp,
			                        angleRight,
			                        new Vector2(n_scaleTemp.x, 
			            			n_scaleTemp.y - (TreeParms.scaleChange + Random.Range(0, TreeParms.scaleRandom)) ),
			                        n_depthTemp+1, "Twin_Right_Branch", n_thisOBJ, Tree_Master.treeMasterGameOBJ, Tree_Master.trunkPrefab, 
			                        Tree_Segment_Script.TreeSegmentType.BRANCH);
			break;
		}
		
	}
}
