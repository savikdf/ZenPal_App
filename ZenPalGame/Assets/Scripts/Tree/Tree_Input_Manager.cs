using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tree_Input_Manager : MonoBehaviour {

	public Slider angleSlider;
	public static float treeInputAngle;
	public Tree_Master master;

    //Controller Stuff
    private bool connected = false;             //Is the controller connected

    private float treeInputMin = -45f;          //The min value that treeInput uses
    private float treeInputMax = 45f;           //The max value that the treeInput uses

    void Awake()
    {
        StartCoroutine(CheckForControllers());
    }

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{ 
		treeInputAngle = angleSlider.value;

//        //If a controller is connected or the current device supports a gyroscope overwrite treeInputAngle with the gyroscope rotation
//        if (connected || SystemInfo.supportsGyroscope)
//        {
//            //Quaternion referenceRotation = Quaternion.identity;
//            Quaternion deviceRotation = DeviceRotation.Get();
//            //Vector3 modifier = new Vector3(0, 0, 1);
//            //Quaternion modifiedRotation = Quaternion.Inverse(Quaternion.FromToRotation(referenceRotation * modifier, deviceRotation * modifier));
//            //modifiedRotation *= deviceRotation;
//
//            //float val = modifiedRotation.eulerAngles.z;
//            float val = deviceRotation.eulerAngles.z;
//
//            treeInputAngle = (val > 180f) ? ((treeInputMin / 180f) * (360f - val)) : ((treeInputMax / 180f) * val);
//            //treeInputAngle = modifiedRotation.eulerAngles.z;
//        }
//        else
//        {
//            //treeInputAngle = angleSlider.value;
//        }
	}

	public void ResetTree()
	{
		angleSlider.value = 0;
		master.RestartGrowth();
	}

    //Track if a controller is connected
    IEnumerator CheckForControllers()
    {
        while (true)
        {
            string[] controllers = Input.GetJoystickNames();
            if (!connected && controllers.Length > 0)
            {
                connected = true;
                Debug.Log("Connected");
            }
            else if (connected && controllers.Length == 0)
            {
                connected = false;
                Debug.Log("Disconnected");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
