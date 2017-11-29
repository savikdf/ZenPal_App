using UnityEngine;
using System.Collections;

public class Branch_Manager : MonoBehaviour {

	public Vector3 branchScale;
	public float timer, timerlengh, totalTime;
	public GameObject branch, camPoint;
	public bool created;
	
	float defualtTime = 0;
	// Use this for initialization
	void Start () 
	{
		
		if(timerlengh == 0)
		{
			timerlengh = defualtTime;
		}
		
		branchScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{

		timer += Time.deltaTime;

		if(timer > timerlengh)
		{
			branchScale.x += .005f;
			branchScale.y = branchScale.x/15;
			timer = 0;
		}
		
		transform.localScale = branchScale;
	}
	
}

