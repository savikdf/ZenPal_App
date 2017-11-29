using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProfileSlider : MonoBehaviour {
	public RectTransform panel, center;
	//public Button[] bttn;
	public Button[] bttn;
	private float[] distance;
	bool isDragging = false;
	int bttnDistance;
	private int minButtonNum;
	[HideInInspector] public bool isBrowsing = false;

	//=======================================================
	//event manager will call these 
	public void StartDrag(){
		isDragging = true;
		
	}
	public void EndDrag(){
		isDragging = false;
		
	}

	//------------------------------------------------------ 

	public void SessionProfilesOpen(){
		int bttnLength = bttn.Length;
		distance = new float[bttnLength];
		bttnDistance = (int)Mathf.Abs (bttn[1].GetComponent<RectTransform>().anchoredPosition.x - 
		                               bttn[0].GetComponent<RectTransform>().anchoredPosition.x);
		isBrowsing = true;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			isBrowsing = !isBrowsing;
			if(isBrowsing)
				SessionProfilesOpen();
		}
		if (isBrowsing) {
			for (int i = 0; i < bttn.Length; i++) {
				distance [i] = Mathf.Abs (center.transform.position.x - bttn [i].transform.position.x);
			}
			float minDistance = Mathf.Min (distance);
			for (int j = 0; j < bttn.Length; j++) {
				if (minDistance == distance [j]) {
					minButtonNum = j;
				}
			}
			if (!isDragging) {
				LerpToBttn (minButtonNum * - bttnDistance);
			}
		}
	}

	void LerpToBttn(int position){
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * 20f);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);
		panel.anchoredPosition = newPosition;
	}

	//-----------------------------------------------------

}
