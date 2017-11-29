using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class  ProfileData {
	static int maxProfiles = 6;		//maybe pay for more profiles? 
	static bool hasCreatedProfile(){
		if (PlayerPrefs.HasKey ("PROFILES")) {
			return true;
		} else
			return false;
	}
	//============================================
	//Checks during app startup, this determines if the player will be brough to the profile creation screen instead of the main menu
	public static bool HasProfiles(){
		//Debug.Log("CHECKING Profile Data. . .");
		return hasCreatedProfile ();
	}

	//used for setting up the very first profile for the app
	public static void InitialProfileSetup(){
		Debug.Log("INITIAL SETUP: Profile 1. . .");
		PlayerPrefs.SetString ("PROFILES", "0"); //this is just used for checking if they have profile data, dont worry about the string its saving
		string p = "Prof";
		string profcheck = string.Format ("{0}{1}", p, "1");

		Profile initialProfile = new Profile();
		//initial setup of variabels
		PlayerPrefs.SetString(profcheck + "Name", "DEFAULT");								//name
		PlayerPrefs.SetInt(profcheck + "Number", 1);										//number
		PlayerPrefs.SetInt(profcheck + "ZenCount", 0);										//zen count
		PlayerPrefsX.SetColor(profcheck + "Color", Color.white);							//color			
		
		initialProfile.profileName = PlayerPrefs.GetString(profcheck + "Name").ToString();	//name
		initialProfile.profileNumber = PlayerPrefs.GetInt(profcheck + "Number");			//number
		initialProfile.profileColor = PlayerPrefsX.GetColor(profcheck + "Color");			//color	

	}

	//==========================================
	//PROFILES START AT 1, not 0. !!!!!!!!!!!!!!!!! dont forget this
	//This is the list the Profile manager will load on startup
	public static List<Profile> ProfileLoadList(){
		//Debug.Log("LOADING Profile List. . .");
		List<Profile> returnProfiles = new List<Profile> ();								//List that will be returned
		string p = "Prof";																	//Used for string formats

		//cycles through saved data to find any existing profiles and add them to the return List<Profiles>
		for (int i = 1; i < maxProfiles; i ++) 
		{		
			//String used for playerprefs profile Loading
			string profcheck = string.Format ("{0}{1}", p, i.ToString());
			//Checking if the data exists, if yes make a new profile with the info and add it to the return list
			//checks if the player name exists, this is what all data checks will use
			if (PlayerPrefs.HasKey (profcheck + "Name")) 
			{
				//Debug.Log(profcheck);				//just to check
				Profile tempProfile = new Profile();
				tempProfile.profileName = PlayerPrefs.GetString(profcheck + "Name").ToString();	//name
				tempProfile.profileNumber = PlayerPrefs.GetInt(profcheck + "Number");			//number
				tempProfile.zenCount = PlayerPrefs.GetInt(profcheck + "ZenCount");				//zen count 	
				tempProfile.profileColor = PlayerPrefsX.GetColor(profcheck + "Color");			//color	
				returnProfiles.Add(tempProfile);
			}		
		
		}
		if (returnProfiles.Count < 1) 
		{
			// make the game loop through the initial profile creation... Shouldn't ever reach this code.
			Debug.Log("ERROR : No/ Missing Profile Data");
		}
		//Debug.Log ("PROFILE LENGTH: " + returnProfiles.Count);
		return returnProfiles;

	}



}
