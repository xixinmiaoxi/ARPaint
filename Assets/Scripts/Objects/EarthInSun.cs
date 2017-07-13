using UnityEngine;
using System.Collections;

public class EarthInSun : MonoBehaviour {

	public GameObject Earth;
	public GameObject Sun;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){

		Earth.GetComponent<Renderer> ().enabled = true;
		Earth.GetComponent<EarthTouch> ().EarthB2.SetActive (true);
		Earth.GetComponent<EarthTouch> ().SetState = 0;
		Sun.SetActive (false);
	}
}
