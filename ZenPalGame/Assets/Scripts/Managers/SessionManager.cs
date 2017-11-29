using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour {
	public static SessionManager instance;
	public enum SessionStates{
		None,
		ProfileChoose,
		TimeChoose,
		ThemeChoose,
		Pre,
		Intra,
		Post
	}
	public enum ThemeStates{
		Classic,
		Theme1,
		Theme2,
		Theme3
	}
	public SessionStates c_SessionState = SessionStates.None;
	public int profileAmout = 0;
	[HideInInspector] public List<Profile> sessionProfiles = new List<Profile> ();							

	public GameObject Object_time, Object_theme;  //object that have the children for all the shit
	public Text timeIndicator;	//display text for the time
	[HideInInspector] public int SessionTime_minutes ;				//variable used for the session time
	public Slider timeSlider;										//Slider for changing the session time
	[HideInInspector] public ThemeStates sessionTheme;				//enum that the tree scene reveives

	public Button continueButton;									//button that continues, will call the ProgressSession()

	//--------------------------------------------------------

	void Awake(){
		DontDestroyOnLoad (gameObject);
		if (instance == null) {
			instance = this;
		}
	}




	//--------------------------------------------------------progression

	public void ProgressSession(){
		if (c_SessionState == SessionStates.ProfileChoose) {
			ProfileManager.instance.CloseProfilesPanels ();
			sessionProfiles = ProfileManager.instance.profileExportList;
			//Debug.Log (sessionProfiles.Count);
			SessionStateSwitch (SessionStates.TimeChoose);
		}
		else if (c_SessionState == SessionStates.TimeChoose) {
			SessionStateSwitch (SessionStates.ThemeChoose);
		}
		else if (c_SessionState == SessionStates.ThemeChoose) {
			OnSwitchToTreeScene();
		}
	}

	//switches the continue button on and off
	public void CheckProgressionAvailability(){
		if (c_SessionState == SessionStates.ProfileChoose) {
			if (profileAmout == 0) {
				continueButton.interactable = false;
			} else
				continueButton.interactable = true;
		}
		else if (c_SessionState == SessionStates.TimeChoose) {
			//this is aready set, no input is required. Will be 5minutes if they dont interact with it
			continueButton.interactable = true;
		}
		else if (c_SessionState == SessionStates.ThemeChoose) {
			continueButton.interactable = true;
		} else {
			continueButton.interactable = false;
		}
	}

	//-----------------------------------------------------switches


	public void SessionStateSwitch(SessionStates toState){
		c_SessionState = toState;
		switch (toState) {
		case SessionStates.ProfileChoose:
			CheckProgressionAvailability ();

			break;
		case SessionStates.TimeChoose:
			SessionTime_minutes = 5;
			Object_time.SetActive (true);
			Object_theme.SetActive (false);
			CheckProgressionAvailability ();

			break;
		case SessionStates.ThemeChoose:
			sessionTheme = ThemeStates.Classic;
			Object_theme.SetActive (true);
			Object_time.SetActive (false);
			CheckProgressionAvailability ();


			break;
		case SessionStates.Pre:


			break;
		case SessionStates.Intra:


			break;
		case SessionStates.Post:


			break;
		case SessionStates.None:
			profileAmout = 0;
			Object_time.SetActive (false);
			Object_theme.SetActive (false);
			ProfileManager.instance.profileExportList.Clear ();

			break;
		}
	
	}

	//---------------------------------BUTTONS

	public void OnSessionButtonHit(){
		//Debug.Log("Session Planning Begin");
		c_SessionState = SessionStates.ProfileChoose;
		//tell the profile manager to Spawn the Profile cards so they can choose which profile to meditate under
		ProfileManager.instance.OnProfilePhaseEnter ();
		SessionStateSwitch (c_SessionState);
	}

	//-------------------------------time and theme

	public void OnTimeSliderChange(){
		//changes the text display
		timeIndicator.text = (timeSlider.value * 5).ToString() + "m";
		SessionTime_minutes = Mathf.RoundToInt(timeSlider.value * 5);
	}

	public void OnThemeChange(){
	
	

	}

	//------------------------------scenes

	public void OnSwitchToTreeScene(){
		//loads the tree scene
		if (sessionProfiles.Count <= 0 && SessionTime_minutes <= 0) {
			//shits fucked if this hits, will loop back to menu. 
			//the code should skip this if()
			GameManager.instance.MainMenuOpen ();
		}else
			Application.LoadLevel (3);
	}

	//================================================================ POST SCENE

	public void PassSessionInformation(){
		//gives the results manager the session data
		//nothing really needed here...

		//incriments the zen count for the profiles that meditated.
		string p = "Prof";	
		Debug.Log (sessionProfiles.Count + " Profiles being proccessed...");
		for (int i = 0; i < sessionProfiles.Count; i++) {		
			string profcheck = string.Format ("{0}{1}", p, sessionProfiles[i].profileNumber.ToString());	//Prof# string format for saving

			int temp = PlayerPrefs.GetInt (profcheck + "Zencount");
			temp += 1;
			PlayerPrefs.SetInt (profcheck + "Zencount", temp);

		}

		ResultsManager.instance.DisplayResultCards ();
		Destroy (gameObject);


	}





}
