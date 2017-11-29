using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GyroDebugging : MonoBehaviour {

    public Text gyroX;
    public Text gyroY;
    public Text gyroZ;

    private float treeInputMin = -45f;          //The min value that treeInput uses
    private float treeInputMax = 45f;           //The max value that the treeInput uses

    void Awake()
    {
        //If this is running in the unity editor or flagged as a development build enable the gyroscope rotation text objects
        //otherwise disable this component
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        gyroX.gameObject.SetActive(true);
        gyroY.gameObject.SetActive(true);
        gyroZ.gameObject.SetActive(true);
#else
        this.enabled = false;
#endif
    }
	
	void Update () {
        //If this is running in the unity editor or flagged as a development build set the gyroscope rotation values on the text objects
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (SystemInfo.supportsGyroscope)
        {
            //Quaternion referenceRotation = Quaternion.identity;
            Quaternion deviceRotation = DeviceRotation.Get();
            //Vector3 modifier = new Vector3(0, 0, 1);
            //Quaternion modifiedRotation = Quaternion.Inverse(Quaternion.FromToRotation(referenceRotation * modifier, deviceRotation * modifier));
            //modifiedRotation *= deviceRotation;

            //float xVal = modifiedRotation.eulerAngles.x;
            //float yVal = modifiedRotation.eulerAngles.y;
            //float zVal = modifiedRotation.eulerAngles.z;
            float xVal = deviceRotation.eulerAngles.x;
            float yVal = deviceRotation.eulerAngles.y;
            float zVal = deviceRotation.eulerAngles.z;

            gyroX.text = ((xVal > 180f) ? ((treeInputMin / 180f) * (360f - xVal)) : ((treeInputMax / 180f) * xVal)).ToString();
            gyroY.text = ((yVal > 180f) ? ((treeInputMin / 180f) * (360f - yVal)) : ((treeInputMax / 180f) * yVal)).ToString();
            gyroZ.text = ((zVal > 180f) ? ((treeInputMin / 180f) * (360f - zVal)) : ((treeInputMax / 180f) * zVal)).ToString();

            //gyroX.text = modifiedRotation.eulerAngles.x.ToString();
            //gyroY.text = modifiedRotation.eulerAngles.y.ToString();
            //gyroZ.text = modifiedRotation.eulerAngles.z.ToString();
        }
#endif
    }
}
