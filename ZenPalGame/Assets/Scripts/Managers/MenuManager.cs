using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public static MenuManager instance;
	public Canvas[] mainCanvases = new Canvas[4];
	[HideInInspector] public MenuInfo.MenuStates c_MenuState = MenuInfo.MenuStates.Main; //only used for gets
	bool canNavigate = true;

	[HideInInspector] public bool isCreatingProfile = false;

	//=========================================

    [SerializeField]
    private GameObject[] defaultButtons = new GameObject[4];

	void Awake(){
		instance = this;
	}

	void Start(){
	}

    

	void Update(){
        
	}

	//========================================



	public void ButtonHit(int toMenuIndex){
		MenuInfo.MenuStates tempState;
		tempState = MenuIndexToMenuStateConversion (toMenuIndex);
		SwitchMenu (tempState);
        //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(defaultButtons[toMenuIndex]);
	}

	MenuInfo.MenuStates MenuIndexToMenuStateConversion(int index){
		//Refer to the canvas holder object for the index values
		if (index == 0) {
			return MenuInfo.MenuStates.Main;
		}
		if (index == 1) {
			return MenuInfo.MenuStates.Profiles;
		}
		if (index == 2) {
			return MenuInfo.MenuStates.Records;
		}
		if (index == 3) {
			return MenuInfo.MenuStates.Session;
		}



		else return MenuInfo.MenuStates.Main;
	}

	//========================================Main switch case for canvas changing 
	public void SwitchMenu(MenuInfo.MenuStates toState){
		//only runs if they are allowed to naviate the menus
		if (canNavigate) {
			switch (toState) {
			case MenuInfo.MenuStates.Main:
			//0
				ExclusiveEnabled (mainCanvases, 0);
				GameManager.instance.MainMenuOpen ();
				break;

			case MenuInfo.MenuStates.Profiles:
			//1
				ExclusiveEnabled (mainCanvases, 1);
				ProfileManager.instance.OnProfilesEnter ();

				break;

			case MenuInfo.MenuStates.Records:
			//2
				ExclusiveEnabled (mainCanvases, 2);

				break;

			case MenuInfo.MenuStates.Session:
			//3
				ExclusiveEnabled (mainCanvases, 3);
				SessionManager.instance.OnSessionButtonHit();

				break;

			}
			c_MenuState = toState;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(defaultButtons[(int)toState]);
		}
	}

	//========================================Disables and Enables for canvases and buttons 
	///This disables all and enabled the one of the passed index
	void ExclusiveEnabled(Canvas[] canvasArray, int exclusiveIndex){
		for (int i = 0; i < canvasArray.Length; i++) {
			if(i == exclusiveIndex){
				canvasArray[i].enabled = true;
			}else{
				canvasArray[i].enabled = false;
			}

		}

	}

	public void SwitchNavigationBool(bool canNav){
		canNavigate = canNav;
		Button[] buttonsTemp = FindObjectsOfType<Button>();
		foreach (Button bTemp in buttonsTemp) {
			if (canNav) {
				bTemp.interactable = true;
			}else{
				bTemp.interactable = false;
			}
		}

	}












}
