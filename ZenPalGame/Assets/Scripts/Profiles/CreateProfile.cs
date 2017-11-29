using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateProfile : MonoBehaviour {
	Canvas creationCanvas;
	public Button[] colorButtons;
	public Image cardImage;
	bool hasColorSet = false;
	public InputField nameInputField;
	Color chosenColor = Color.white;
	int newProfileNumber = 0;
	public Button createButton;

    [SerializeField]
    private GameObject createProfileButton;
    [SerializeField]
    private GameObject viewProfileButton;

	bool hasNameSet(){
		if (nameInputField.text != "" && nameInputField.text.Length > 0) {
			return true;
		}
		else return false;
	}

	//=============================================================
	void Awake(){
		creationCanvas = GetComponent<Canvas> ();
	}
	void Start(){
		creationCanvas.enabled = false;
	}

	//============================================================Start of creation
	public void OnCreationStarted(){
		//checks for profile data, if none, sets number to 1, else adds to the existing data 
		newProfileNumber = 1;
		if (ProfileData.ProfileLoadList () != null) {
			newProfileNumber = ProfileData.ProfileLoadList ().Count + 1;
		} 
		Debug.Log ("Creating Profile #" + newProfileNumber.ToString());

		//sets the button colours
		for (int i = 0; i < colorButtons.Length; i++) {
			colorButtons [i].GetComponent<Image> ().color = ProfileManager.instance.profileColours [i];
		}
		nameInputField.text = "";
		cardImage.color = Color.white;
		creationCanvas.enabled = true;
		//checks if continuation is possible, this should turn it off since there is no data yet 
		CheckCreateEnabled ();
	}

	//------


	public void OnCreationEnded(){	
		nameInputField.text = "";
		cardImage.color = Color.white;
		creationCanvas.enabled = false;	
	}
	//===========================================================Name and Color things

	public void ColorButtonHit(int buttonIndex){
		cardImage.color = ProfileManager.instance.profileColours [buttonIndex];
		chosenColor = ProfileManager.instance.profileColours [buttonIndex];
		hasColorSet = true;	
		Debug.Log ("Colour hit");
		CheckCreateEnabled ();
	}

	public void NameInputEvent(){
		CheckCreateEnabled ();
	}

	//==========================================================Button checks

	public void CheckCreateEnabled(){
		if (hasNameSet () && hasColorSet) {
			//enables the create buttton if they have a name and a colour
			createButton.interactable = true;
		} else
			createButton.interactable = false;
	}

	public void CreateButtonHit(){	
		Debug.Log ("Creating Profile #" + newProfileNumber.ToString());	
		SaveProfile (nameInputField.text, newProfileNumber, chosenColor);

        createProfileButton.gameObject.SetActive(true);
        viewProfileButton.gameObject.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(createProfileButton);
	}

	//==========================================================Profile saving

	private void SaveProfile(string n_name, int n_number, Color n_color){
		//check if the profile number doesn't conflict, save that shit
		if (n_number == 1) {
			ProfileData.InitialProfileSetup ();
		}
		string p = "Prof";
		string profcheck = string.Format ("{0}{1}", p, n_number.ToString());	//should save it to the n_number value profile


		//initial setup of variabels
		PlayerPrefs.SetString (profcheck + "Name", 	n_name);								//name set
		PlayerPrefs.SetInt (profcheck + "Number", 	n_number);								//number set
		PlayerPrefs.SetInt (profcheck + "ZenCount", 0);										//zen count set
		PlayerPrefsX.SetColor (profcheck + "Color", n_color);								//color set

		//------
//		these values are just in case, future animation possibly?

//		Profile tempProfile = new Profile ();
//		tempProfile.profileName = PlayerPrefs.GetString (profcheck + "Name");				//name get
//		tempProfile.profileNumber = PlayerPrefs.GetInt (profcheck + "Number");				//number get
//		tempProfile.zenCount = PlayerPrefs.GetInt (profcheck + "ZenCount")					//zen count get
//		tempProfile.profileColor = PlayerPrefsX.GetColor (profcheck + "Color");				//color get

		Debug.Log ("SAVING PROFILE #" + n_number.ToString () + " . . .");

		OnSaveComplete (); 

	}

	void OnSaveComplete(){
		//reset all local shit here:
		Debug.Log ("SAVE COMPLETE");	
		
		//disables the cards
		OnCreationEnded();
		ProfileManager.instance.BackProfileButtonHit ();
	}
}
