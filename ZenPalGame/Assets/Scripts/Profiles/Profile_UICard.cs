using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Profile_UICard : MonoBehaviour {
	public Image backgroundCard, selectionOutline;
	public Text profileNameText, profileNumber, zenCount;
	Profile assignedProfile;
	bool isSelected = false;

	public void InitializeProfileCard(Profile profile){
		//syncing the profile info to the UI info
		assignedProfile = profile;

		backgroundCard.color = profile.profileColor;
		profileNumber.text = profile.profileNumber.ToString();
		zenCount.text = profile.zenCount.ToString ();

		string p = "Prof";		
		string profcheck = string.Format ("{0}{1}", p, profile.profileNumber.ToString());	//Prof# string format for saving
		int temp = PlayerPrefs.GetInt (profcheck + "Zencount");
		zenCount.text = temp.ToString ();

		profileNameText.text = profile.profileName;
		backgroundCard.color = profile.profileColor;
		isSelected = false;
		selectionOutline.enabled = isSelected;
	}

	//This will run only in the session setting 
	//profile becomes a button which comunicates the the Prof_Man and feeds it the session list
	public void OnProfileButtonPressed(){
		if (SessionManager.instance.c_SessionState == SessionManager.SessionStates.ProfileChoose) {
			SelectToggle ();
			if (isSelected) {
				ProfileManager.instance.OnProfileSelected (assignedProfile, 1);
				//Debug.Log (assignedProfile.profileName.ToString () + " Was Pressed.");
			} else if (!isSelected) {
				ProfileManager.instance.OnProfileSelected (assignedProfile, -1);
			}
		} else {

		}
	}

	//button highlight toggle
	public void SelectToggle(){
		if (!isSelected && SessionManager.instance.profileAmout < 2) {	
			isSelected = true;
			selectionOutline.enabled = isSelected;
			SessionManager.instance.profileAmout++;
		}
		else if (isSelected) {	
			isSelected = false;
			selectionOutline.enabled = isSelected;
			SessionManager.instance.profileAmout--;
		}
		SessionManager.instance.CheckProgressionAvailability ();

	}


//	public void HardSetColour(Color toColor){
//		backgroundCard.color = toColor;
//		//saving the data
//		string p = "Prof";
//		string profcheck = string.Format ("{0}{1}", p, profileNumber.ToString());
//		PlayerPrefsX.SetColor(profcheck + "Color", toColor);
//	}



}
