using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultCard : MonoBehaviour {
	public Text cardName, cardNumber, cardZenCount;
	public Image cardImage;

	public void InitializeResultCard(Profile initProfile){
		cardName.text = initProfile.profileName;
		cardNumber.text = initProfile.profileNumber.ToString ();
		cardImage.color = initProfile.profileColor;	
	
	//zen count:
		//probs a better way to do this, this will do for now ay
		string p = "Prof";		
		string profcheck = string.Format ("{0}{1}", p, initProfile.profileNumber.ToString());	//Prof# string format for saving
		int temp = PlayerPrefs.GetInt (profcheck + "Zencount");
		cardZenCount.text = temp.ToString ();	
	}




}
