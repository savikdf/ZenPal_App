using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//=========================================================== PROFILE CLASS
//===========================================================
//this is the profile info that will be used in the game for each individual user.
public  class Profile {
	public Profile_UICard profileCard;	//for UI info display
	public string profileName;			//profile name
	public int profileNumber;			//profile number
	public int zenCount;				//how many times they've meditated
	public Color profileColor;			//possible color that the user can choose for their profile?

}
//===========================================================
//=========================================================== PROFILE MANAGER

public class ProfileManager : MonoBehaviour {

	public static ProfileManager instance;
	public enum ProfileStates{
		SessionAdd,
		View,
		Creation,
		Remove,
		None
	};
	[HideInInspector] public ProfileStates c_ProfileState = ProfileStates.None;
	public List<Profile> profileExportList = new List<Profile> ();							
	public GameObject profileUICard_Prefab;										//prefabs of the profile cards
	public Canvas sessionProfileCanvas;												//Canvas the cards will be displayed on
	[HideInInspector]public List<Profile> profileList = new List<Profile>();	//List of profiles that exist
	public Color[] profileColours = new Color[6];

	public GameObject scrollPanel;

	//Profile menu buttons
	public Button viewProfileButton, createProfileButton;

	//Creation things
	public Canvas CreationCanvas;


    [SerializeField]
    private GameObject profileCreationNamePanel;


    //Used for navigation
    [SerializeField]
    private Button cancelButton;        //Cancel button in session menu
    [SerializeField]
    private Button continueButton;      //Continue button in session menu
    [SerializeField]
    private Button backButton;          //Back button in profile view menu
	//--------------------------------starts

	void Awake(){
		instance = this;
	}

	void Start(){
		sessionProfileCanvas.enabled = false;
		CreationCanvas.enabled = false;
	}
		

	//----------------------------------
	//runs when the profiles menu gets opened
	public void OnProfilesEnter(){
		if (!ProfileData.HasProfiles ()) {
			//MenuManager.instance.SwitchNavigationBool (false);
			OnCreateProfileButton ();
		}
	}


	//brings up the creation menu thing
	public void EnableCreationPanel(bool temp){
		CreationCanvas.enabled = temp;
		if(temp && SessionManager.instance.c_SessionState == SessionManager.SessionStates.None)
			CreationCanvas.GetComponent<CreateProfile> ().OnCreationStarted ();
	}


	//----------------------------------Creating Panels
	//runs when a session is started, to choose your profile
	public void OnProfilePhaseEnter(){
		//show profile cards
		sessionProfileCanvas.enabled = true;
		List<Profile> tempProfList = ProfileData.ProfileLoadList ();
		Profile_UICard[] uiCardArray = new Profile_UICard[tempProfList.Count];
		//creates the profiles under the canvas to be selected by the user
		for (int i = 0; i < tempProfList.Count; i ++){
			GameObject uiCardTemp = (GameObject) Instantiate(profileUICard_Prefab, Vector3.zero, Quaternion.identity);
			Profile_UICard temp = uiCardTemp.GetComponent<Profile_UICard>();
			temp.InitializeProfileCard(tempProfList[i]);
			uiCardArray[i] = temp;
		}
		SpaceProfiles (uiCardArray);
        AssignProfileNavigation (uiCardArray);

		//swtiches the current state of the profile viewing...
		//will be SessionAdd or View
		if (SessionManager.instance.c_SessionState == SessionManager.SessionStates.ProfileChoose) {
			c_ProfileState = ProfileStates.SessionAdd;
		} else if (SessionManager.instance.c_SessionState == SessionManager.SessionStates.None) {
			c_ProfileState = ProfileStates.View;
			foreach (Profile_UICard disTemp in uiCardArray) {
				disTemp.GetComponent<Button> ().enabled = false;
			}
		}
	}

	//runs when the profile phase is done
	public void OnProfilePhaseClose(){
		//tell session manager what profiles are being used

		//disable profile cards
		CloseProfilesPanels();

	}

	//----------------------------------Spacing Panels
	void SpaceProfiles(Profile_UICard[] toSpaceList){
		Debug.Log("Positioning " + toSpaceList.Length.ToString() + " Cards...");

		for (int i = 0; i < toSpaceList.Length; i++) 
		{
			//spacing and instantiating the panels
			toSpaceList[i].gameObject.transform.SetParent(scrollPanel.transform);
			toSpaceList[i].gameObject.transform.localScale = new Vector3(3.3f, 4.5f, 1f);
			toSpaceList[i].GetComponent<RectTransform>().transform.position = new Vector3((Screen.width/4f * i) + Screen.width / 8f, Screen.height/2f, 0);		
		}

		Debug.Log("Finished Positioning");
	}
	public void CloseProfilesPanels(){
		//this is for session to main
		foreach (Transform child in scrollPanel.transform) {
			Destroy (child.gameObject);
		}
	}


	//----------------------------------profile selection
	public void OnProfileSelected(Profile chosenProfile, int selectIndex){
		if (MenuManager.instance.c_MenuState == MenuInfo.MenuStates.Session) {
			if (selectIndex == 1) {
				profileExportList.Add (chosenProfile);
			}
			if (selectIndex == -1) {
				profileExportList.Remove (chosenProfile);
			}
		}
	}
		
	//----------------------------------Buttons

	//back button
	public void BackProfileButtonHit(){
		if (c_ProfileState == ProfileStates.View) {
			CloseProfilesPanels ();
			createProfileButton.gameObject.SetActive(true);
			viewProfileButton.gameObject.SetActive(true);	
			c_ProfileState = ProfileStates.None;
		}else if(c_ProfileState == ProfileStates.Creation){
			//close the profiles
			CreationCanvas.GetComponent<CreateProfile>().OnCreationEnded();
			Debug.Log ("Creation Ended");
			createProfileButton.gameObject.SetActive(true);
			viewProfileButton.gameObject.SetActive(true);
			c_ProfileState = ProfileStates.None;
		} 
		else {
			MenuManager.instance.SwitchMenu (MenuInfo.MenuStates.Main);
		}
	}

	//create profile button 
	public void OnCreateProfileButton(){
		EnableCreationPanel (true);
		createProfileButton.gameObject.SetActive(false);
		viewProfileButton.gameObject.SetActive(false);
        c_ProfileState = ProfileStates.Creation;

        //set active thing to be the name panel
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(profileCreationNamePanel);
	}

	//view profile button
	public void OnViewProfiles(){
		//will bring up the panels, they wont work due to the session manager however. 
		//this is good.
		createProfileButton.gameObject.SetActive(false);
		viewProfileButton.gameObject.SetActive(false);		
		OnProfilePhaseEnter ();

        ////set active thing to be the name panel
        //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(profileCreationNamePanel);
    }

    //----------------------------------Buttons
    private void AssignProfileNavigation(Profile_UICard[] profileList)
    {
        
        if(profileList.Length > 0)
        {
            Navigation nav = cancelButton.navigation;
            nav.selectOnUp = profileList[0].gameObject.GetComponent<Selectable>();
            nav.selectOnDown = profileList[0].gameObject.GetComponent<Selectable>();
            cancelButton.navigation = nav;

            nav = continueButton.navigation;
            nav.selectOnUp = profileList[0].gameObject.GetComponent<Selectable>();
            nav.selectOnDown = profileList[0].gameObject.GetComponent<Selectable>();
            continueButton.navigation = nav;

            nav = backButton.navigation;
            nav.selectOnUp = profileList[0].gameObject.GetComponent<Selectable>();
            nav.selectOnDown = profileList[0].gameObject.GetComponent<Selectable>();
            backButton.navigation = nav;

            nav = profileList[0].gameObject.GetComponent<Selectable>().navigation;
            nav.selectOnLeft = profileList[profileList.Length - 1].gameObject.GetComponent<Button>();
            nav.selectOnRight = profileList[1].gameObject.GetComponent<Button>();
            if (MenuManager.instance.c_MenuState == MenuInfo.MenuStates.Profiles)
            {
                nav.selectOnDown = backButton;
                nav.selectOnUp = backButton;
            }
            else if (MenuManager.instance.c_MenuState == MenuInfo.MenuStates.Main)         //Apparently the profile select on session is the "Main" state
            {
                nav.selectOnDown = cancelButton;
                nav.selectOnUp = cancelButton;
            }

            profileList[0].gameObject.GetComponent<Selectable>().navigation = nav;

            for(int i = 1; i < profileList.Length; i++)
            {
                //nav = profileList[i].gameObject.GetComponent<Selectable>().navigation;
                nav.selectOnLeft = profileList[i - 1].gameObject.GetComponent<Button>();
                if(i == profileList.Length - 1)
                {
                    nav.selectOnRight = profileList[0].gameObject.GetComponent<Button>();
                    
                }
                else
                {
                    nav.selectOnRight = profileList[i + 1].gameObject.GetComponent<Button>();
                }
                //nav.selectOnDown = continueButton;
                //nav.selectOnUp = continueButton;
                profileList[i].gameObject.GetComponent<Selectable>().navigation = nav;
            }
        }
    }
}
