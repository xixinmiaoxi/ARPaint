using UnityEngine;
using System.Collections;

public class EarthTouch : MonoBehaviour {

	public GameObject EarthB2;

	public GameObject Sun;

	public int SetState = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(SetState==1||SetState==2){
		transform.Rotate(0,25*Time.deltaTime,0,Space.Self );
		}
	}

	void OnMouseDown(){

		if (SetState == 0) {
			
			SetState = 1;
		} else if (SetState == 1) {
			EarthB2.SetActive (false);
			SetState = 2;
		} else if (SetState == 2) {
			Sun.SetActive (true);
			gameObject.GetComponent<Renderer> ().enabled = false;
			SetState = 3;
		} else if (SetState == 3) {
			gameObject.GetComponent<Renderer> ().enabled = true;
			EarthB2.SetActive (true);
			Sun.SetActive (false);
			SetState = 0;
		}
	}


}
