using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	[HideInInspector] public bool isDevMode = false;
	public Canvas devCanvas;
	//=========================================================

	void Awake(){
		instance = this;
	}

	void Start(){
		//disabling cheat canvas
		devCanvas.enabled = false;

		//If the system has profile data it will go to the main, otherwise it will go the profiles menu
		if (ProfileData.HasProfiles ()) 
		{
			MenuManager.instance.SwitchMenu (MenuInfo.MenuStates.Main);
		} else
		{
			MenuManager.instance.SwitchMenu (MenuInfo.MenuStates.Profiles);
		}

		SessionManager.instance.SessionStateSwitch (SessionManager.SessionStates.None);

	}
	//========================================================= State Things
	public void MainMenuOpen(){
		SessionManager.instance.SessionStateSwitch (SessionManager.SessionStates.None);
		ProfileManager.instance.CloseProfilesPanels ();
	}


	//========================================================= CHEAT THINGS

	void Update(){
		if (Input.GetKeyDown (KeyCode.Equals)) {
			isDevMode = !isDevMode;
			devCanvas.enabled = isDevMode;
			MenuManager.instance.SwitchNavigationBool(true);
		}

		if (Input.GetKeyDown (KeyCode.Delete) && isDevMode) {
			Debug.Log("! ! ! DELETING ALL SAVED DATA ! ! !");
			PlayerPrefs.DeleteAll();
		}
	}

	public void LoadTreeScene(){
		Application.LoadLevel (3);
	}


	void OnGUI(){
		if(isDevMode)
			GUILayout.Label ("DEV MODE");
	}


}
