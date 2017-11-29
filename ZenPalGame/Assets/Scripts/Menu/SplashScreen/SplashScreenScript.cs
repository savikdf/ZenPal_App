using UnityEngine;
using System.Collections;

public class SplashScreenScript : MonoBehaviour {
	void Start(){
		GetComponent<Animator> ().SetBool ("CanPlay", true);
	}
	public void SplashFinish(){
		GetComponent<Animator> ().SetBool ("CanPlay", false);
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
