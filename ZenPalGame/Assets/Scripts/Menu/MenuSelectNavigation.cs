using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MenuSelectNavigation : MonoBehaviour {

    [System.Serializable]
    public class MyEventType : UnityEvent { }

    public GameObject navigateTo;
    [SerializeField]
    private bool navigateOnCancel = false;
    public MyEventType OnEvent;


	void Update () {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            if (navigateOnCancel)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(navigateTo);
                    OnEvent.Invoke();
                }
            }
            else
            {
                if (Input.GetButtonDown("Submit"))
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(navigateTo);
                    OnEvent.Invoke();
                }
            }
        }
	}
}
