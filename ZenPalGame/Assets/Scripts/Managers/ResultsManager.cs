using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class ResultsManager : MonoBehaviour {
	public static ResultsManager instance;
	public GameObject resultCard_Prefab;
	public Canvas resultsCanvas;


	void Awake(){
		instance = this;
	}

	void Start(){
		if (SessionManager.instance != null) {
			GatherInfo ();
		}
	}

	//----------------------------------------------Gathring Data
	void GatherInfo(){
	//pulls the info out of the session manager then destroys it
		SessionManager.instance.PassSessionInformation();	
	}

	//-----------------------------------------------Displaying Info
	//this is called from the session manager, during the GatherInfo()
	public void DisplayResultCards(){
		RectTransform[] spacingArray = new RectTransform[SessionManager.instance.sessionProfiles.Count];
		for (int i = 0; i < SessionManager.instance.sessionProfiles.Count; i ++){
			GameObject resultCard = (GameObject) Instantiate(resultCard_Prefab, Vector3.zero, Quaternion.identity);
			ResultCard temp = resultCard.GetComponent<ResultCard>();
			spacingArray [i] = temp.gameObject.GetComponent<RectTransform> ();
			temp.InitializeResultCard(SessionManager.instance.sessionProfiles[i]);
		}
		SpaceResultCards(spacingArray);
	}

	void SpaceResultCards(RectTransform[] resultCardRecArray){
		for (int i = 0; i < resultCardRecArray.Length; i++) {
			resultCardRecArray [i].transform.parent = resultsCanvas.transform;
			resultCardRecArray[i].transform.position = new Vector3((Screen.width/4f * i) + Screen.width / 8f, Screen.height/2f, 0);
			//toSpaceList[i].GetComponent<RectTransform>().transform.position = new Vector3((Screen.width/4f * i) + Screen.width / 8f, Screen.height/2f, 0);		

		}
	}

	//----------------------------------------------Buttons
	public void OnReturnToMenuButton(){
		Application.LoadLevel (1);
	}

}
