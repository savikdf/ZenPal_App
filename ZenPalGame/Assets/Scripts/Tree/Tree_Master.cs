using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

//This Class will Keep track of the currently growing Tree, passing information for the base
public class Tree_Master : MonoBehaviour {

	[HideInInspector]
	public static GameObject trunkPrefab, leafPref, anchorPoint, branchSegment, endSeg, StartSeg;
	//public Vector3   trunkTopOffset = new Vector3(0, 1, 0);

	Transform root;
	public static AudioClip TreeGrowFoley;
	public static Vector2 growthMax = new Vector2 (); 

	public static GameObject treeMasterGameOBJ;
	

	public Vector2 scaleRate;
	//Tree Transform Information
	public Vector3 startPos; 
	public static List<GameObject> allTreeSegment = new List<GameObject>();
	public static List<GameObject> anchorList = new List<GameObject>();
	public static List<GameObject> growthList = new List<GameObject>();
	public static List<GameObject> creatorListMaster = new List<GameObject>();
	public static List<GameObject> treeTrunkList = new List<GameObject>();
	public static List<GameObject> segmentMasters = new List<GameObject>();
	// Use this for initialization
	

	void Awake () 
	{
		//treeMasterGameOBJ = this.gameObject;
		//Loading Tree Main Growing Segment 
		GameObject trunkPrefabLoad = Resources.Load("Tree_GameObject", typeof(GameObject)) as GameObject;
		trunkPrefab = trunkPrefabLoad;

		//Loading Tree Main Growing Segment 
		GameObject leafPrefLoad = Resources.Load("Leaf_GameObject", typeof(GameObject)) as GameObject;
		leafPref = leafPrefLoad;

		GameObject AnchorPointLoad = Resources.Load("AnchorPoint", typeof(GameObject)) as GameObject;
		anchorPoint = AnchorPointLoad;

		GameObject branchSegmentLoad = Resources.Load("Brach_Segment_GameObject", typeof(GameObject)) as GameObject;
		branchSegment = branchSegmentLoad;

		AudioClip TreeGrowFoleyLoad = Resources.Load("Audio/Fart2", typeof(AudioClip)) as AudioClip;
		TreeGrowFoley = TreeGrowFoleyLoad;

		root = transform;

		scaleRate = TreeParms.growthRate;
	}
		void Start () 
	{
		//Let The Games BEGIN!!!!
		//Starts the intital tree grow chain
		MakeBaseTree(root.position, 0, new Vector2(0,0), 0);

	}
	


	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		
		}

		if(Input.GetKeyDown(KeyCode.Equals))
		 {
			Application.LoadLevel(4);
		}
		if(allTreeSegment.Count != 0)
		{
		for(int i = 0; i < allTreeSegment.Count; i++)
		{

		}
		}
	
	}
	
	public void RestartGrowth()
	{

		allTreeSegment.Clear();
		anchorList.Clear();
		growthList.Clear();
		creatorListMaster.Clear();
		treeTrunkList.Clear();
		segmentMasters.Clear();

		Tree_Input_Manager.treeInputAngle = 0;
		GetComponent<Tree_Growth_Manager>().ResetTime();

		Destroy(StartSeg);

		MakeBaseTree(root.position, 0, new Vector2(0,0), 0);
	}
	

	void MakeBaseTree(Vector3 startPosTemp, float angleTemp, Vector2 scaleTemp, int depthTemp)
	{
		Debug.Log("StartedTree");
		//create a quaternion specified with euler angles where we rotate around 'x'
		Quaternion rot = Quaternion.Euler( 0, 0, angleTemp);// *  Quaternion.AngleAxis( Random.Range(0,360), Vector3.up);

		//make a trunk
		Vector3 topPos = startPosTemp + (rot * (TreeParms.trunkTopOffset * scaleTemp.y));

		GameObject obj = (GameObject)GameObject.Instantiate(trunkPrefab, startPos, rot);

		GameObject anchorRootObj = (GameObject)GameObject.Instantiate(anchorPoint, startPosTemp, rot);

		anchorRootObj.name = "RootAnchor";
		anchorRootObj.GetComponent<PositionManager>().curChild = obj.transform;
	
		GameObject MainTree = CreateTree();
		StartSeg = MainTree;

		//Passing Over Current Tree Information to the Trunk
        Tree_Segment_Script objSegment = obj.GetComponent<Tree_Segment_Script>();
        objSegment.startPos = topPos;
        objSegment.angle = angleTemp;
        objSegment.scale = TreeParms.growthSplitPoint;
        objSegment.depth = depthTemp;
        objSegment.root = this.transform;
        objSegment.master = GetComponent<Tree_Master>();
        objSegment.treeSegmentTypes = Tree_Segment_Script.TreeSegmentType.ROOT;
		objSegment.pos = anchorRootObj.transform;

		obj.GetComponent<Segment_GrowthState>().growthSplitPoint =  TreeParms.growthSplitPoint;
        obj.GetComponent<Segment_GrowthState>().growthSplitPoint = TreeParms.growthSplitPoint;
		obj.transform.parent = anchorRootObj.transform;

		anchorRootObj.transform.parent = MainTree.transform;

		obj.gameObject.name = "RootTrunk";

		anchorList.Add(anchorRootObj);
		allTreeSegment.Add(obj);
		growthList.Add(obj);
	}

	GameObject CreateTree()
	{
		GameObject tree = new GameObject();
		tree.name = "Tree";
		tree.transform.localScale = new Vector3 (1,1,1);
		tree.AddComponent<Segment_GrowthState>();
		tree.AddComponent<Tree_Segment_Script>();
		tree.GetComponent<Tree_Segment_Script>().treeSegmentTypes = Tree_Segment_Script.TreeSegmentType.TREE;
		tree.GetComponent<Tree_Segment_Script>().isTree = true;
		treeMasterGameOBJ = tree;
		tree.GetComponent<Segment_GrowthState>().treeStages = Segment_GrowthState.TreeStage.GROWING;
		//growthList.Add(tree);

		return tree;
	}
    public static void AddSegment(GameObject obj)
    {
		growthList.Add(obj);
		allTreeSegment.Add(obj);
    }
}