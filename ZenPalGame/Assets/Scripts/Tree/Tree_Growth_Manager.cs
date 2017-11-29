using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tree_Growth_Manager : MonoBehaviour {
	
	public List<GameObject> growthList;
	public float spawnTime, currentTime, spawnChance;
	public int curIndex;
	// Use this for initialization
	void Awake () 
	{
		growthList = Tree_Master.growthList;
	}
	
	// Update is called once per frame
	void Update () 
	{

		currentTime += Time.deltaTime;

		if(Tree_Master.growthList.Count != 0)
		{

		for(int i = 0; i <  Tree_Master.growthList.Count; i++)
		{

			//Grabbing Current Tree Segment's Growth Information
			//Incriments Threw and Increases Scale According to Current Segment State
			//Spurt = Newly Created
			//Growth = Global Growth Rate For all Segments
			//Paused = Stops Growing

			Segment_GrowthState trunkGrowthState =  Tree_Master.growthList[i].GetComponent<Segment_GrowthState>();
			
			Tree_Segment_Script trunkSeginfo =   Tree_Master.growthList[i].GetComponent<Tree_Segment_Script>();
			Vector2 tempVector = new Vector2(trunkGrowthState.transform.localScale.x,trunkGrowthState.transform.localScale.y);
			

			if(currentTime >= spawnTime)
			{
					//==========================================
					//Creation of Master Branches at set intervals
					//==========================================
					curIndex = Random.Range(0, Tree_Master.treeTrunkList.Count - 1);

					if(curIndex >  Tree_Master.treeTrunkList.Count - 1 || curIndex < 0)
					{
						curIndex = 0;
					}
						GameObject branchCreator = Tree_Master.treeTrunkList[curIndex].gameObject;

						branchCreator.GetComponent<Tree_Segment_Script>().createBranch = true;
						Tree_Master.creatorListMaster.Add(branchCreator);

						currentTime = 0;
			}


			if(SessionManager.instance != null)
			{
			if(currentTime >= SessionManager.instance.SessionTime_minutes * 60)
			{
				trunkGrowthState.treeStages = Segment_GrowthState.TreeStage.PUASED;
			}
			}
			else
			{

			}

			
			//controls splitting when a segment has reached a minimum scale

			if(trunkGrowthState.hitEnd && trunkGrowthState.transform.localScale.y  >= trunkGrowthState.growthSplitPoint.y  && !trunkSeginfo.isTree)
			{

				trunkSeginfo.nextSegmentMade = false;
				trunkGrowthState.hitEnd = false;
				Tree_Master.creatorListMaster.Add(trunkSeginfo.gameObject);
			}

            //This will just toggle between the first two results after the scale meets the condition, is this intended?
			if (trunkGrowthState.transform.localScale.y >= trunkGrowthState.growthSplitPoint.y && !trunkGrowthState.hitEnd && !trunkSeginfo.isTree)
			{
					
				Tree_Master.creatorListMaster.Add(trunkSeginfo.gameObject);

				trunkGrowthState.treeStages = Segment_GrowthState.TreeStage.GROWING;
				trunkGrowthState.growthSplitPoint = new Vector2 (trunkGrowthState.growthSplitPoint.x, trunkGrowthState.growthSplitPoint.y /2);
			}

			if(trunkGrowthState.growthSplitPoint.y < TreeParms.minScale && trunkGrowthState.treeStages == Segment_GrowthState.TreeStage.GROWING && !trunkSeginfo.isTree || 
			   trunkGrowthState.transform.localScale.y > trunkGrowthState.growthSplitPoint.y && trunkGrowthState.treeStages == Segment_GrowthState.TreeStage.GROWING && !trunkSeginfo.isTree)
			{

			}
			


			if(trunkGrowthState.treeStages == Segment_GrowthState.TreeStage.SPURT)
			{
				//If the Segment Hits its Current Split Point
				//It will Slow Down and Create a new Segment
//				trunkSeginfo.GetComponent<SpriteRenderer>().color = new Color (trunkSeginfo.GetComponent<SpriteRenderer>().color.r, 
//				                                                               trunkSeginfo.GetComponent<SpriteRenderer>().color.g, 
//				                                                               trunkSeginfo.GetComponent<SpriteRenderer>().color.b, 
//				                                                               (float)tempVector.y * 2);

				tempVector.y = Mathf.Lerp(tempVector.y, tempVector.y + TreeParms.scaleChange, Time.deltaTime * TreeParms.growthRate.y);
				tempVector.x = Mathf.Lerp(tempVector.x, trunkGrowthState.transform.localScale.y, Time.deltaTime * TreeParms.growthRate.y);
				trunkGrowthState.transform.localScale =  new Vector3 (tempVector.x, tempVector.y, 1 );
				
			}

			if(trunkSeginfo.isMasterBranch && trunkGrowthState.treeStages == Segment_GrowthState.TreeStage.GROWING)
			{
				Vector2 tempVector2 = new Vector2(trunkSeginfo.masterSegGroup.transform.localScale.x,	trunkSeginfo.masterSegGroup.transform.localScale.y);
				tempVector2.y = Mathf.Lerp(tempVector2.y, tempVector2.y + TreeParms.scaleChange, Time.deltaTime * TreeParms.globalGrowth.y);
				tempVector2.x = Mathf.Lerp(tempVector2.x, trunkSeginfo.masterSegGroup.transform.localScale.y, (Time.deltaTime * TreeParms.globalGrowth.y) / 2);
				trunkSeginfo.masterSegGroup.transform.localScale =  new Vector3 (tempVector2.x, tempVector2.y, 1 );
			}
			else
			{

			if(trunkGrowthState.treeStages == Segment_GrowthState.TreeStage.GROWING && 
			        trunkSeginfo.transform.parent != trunkSeginfo.masterSegment.GetComponent<Tree_Segment_Script>().masterSegGroup && trunkGrowthState.hitEnd)
			{

				tempVector.y = Mathf.Lerp(tempVector.y, tempVector.y + TreeParms.scaleChange, Time.deltaTime * TreeParms.globalGrowth.y);
				tempVector.x = Mathf.Lerp(tempVector.x, trunkGrowthState.transform.localScale.y, Time.deltaTime * TreeParms.globalGrowth.y);
				trunkGrowthState.transform.localScale =  new Vector3 (tempVector.x, tempVector.y, 1 );
			}

			}
		
		}
		}

		
		}

	public void ResetTime()
	{
		currentTime = 0;
	}

	}
