using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tree_Color_Manager : MonoBehaviour {

	public List<GameObject> TreeSegment = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		TreeSegment = Tree_Master.allTreeSegment;
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i = 0; i < TreeSegment.Count; i++)
		{
			float alpha = ((float)i)/TreeSegment.Count;

			TreeSegment[i].GetComponentsInChildren<SpriteRenderer>()[0].color = new Color (1,1,1, 1 - alpha);
			TreeSegment[i].GetComponentsInChildren<SpriteRenderer>()[1].color = new Color (1,1,1, 1 - alpha);

		}
	
	}
}
